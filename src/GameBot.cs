using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Configuration;
using GameATron4000.Dialogs;
using GameATron4000.Models;
using GameATron4000.Scripting;
using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameATron4000
{
    public class GameBot : IBot
    {
        private readonly BotServices _services;
        private readonly GameBotAccessors _stateAccessors;
        private readonly LUISOptions _luisOptions;

        public GameBot(BotServices services, GameBotAccessors stateAccessors, IOptions<LUISOptions> luisOptionsAccessor)
        {
            _services = services;
            _stateAccessors = stateAccessors;
            _luisOptions = luisOptionsAccessor.Value;
        }

        public async Task OnTurnAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            // Load the metadata for the game.
            var gameInfoJson = File.ReadAllText("Gameplay/game.json");
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(gameInfoJson);

            // Establish dialog context from the game info.
            var dialogSet = CreateDialogSet(gameInfo);
            var dc = await dialogSet.CreateContextAsync(context, cancellationToken);

            if (dc.ActiveDialog == null)
            {
                if (context.Activity.Type is ActivityTypes.ConversationUpdate)
                {
                    foreach (var newMember in context.Activity?.MembersAdded)
                    {
                        if (newMember.Id == context.Activity.Recipient.Id)
                        {
                            // Add some items to the inventory for the player to start with.
                            await _stateAccessors.InventoryItemsAccessor.SetAsync(
                                context, gameInfo.InitialInventory);

                            // And send a GameStarted to the client that contains the inventory items.
                            await context.SendActivityAsync(new ActivityFactory(gameInfo).GameStarted(dc));

                            // Start the game's first room.
                            await dc.BeginDialogAsync(gameInfo.InitialRoom);
                        }
                    }
                }
            }
            else if (context.Activity.Type is ActivityTypes.Message)
            {
                // get intent and entity from LUIS (if enabled).
                if (_luisOptions.Enabled)
                {
                    // TODO Trial 3: Add call to LUIS service
                    string command = await DetermineCommandAsync(context, cancellationToken);
                    if (!string.IsNullOrEmpty(command))
                    {
                        context.Activity.Text = command;
                    }
                }

                await dc.ContinueDialogAsync();
            }

            // Save any changes back to conversation state.
            await _stateAccessors.ConversationState.SaveChangesAsync(context, false, cancellationToken);
        }

        private DialogSet CreateDialogSet(GameInfo gameInfo)
        {
            var dialogSet = new DialogSet(_stateAccessors.DialogStateAccessor);
            var roomParser = new RoomParser(gameInfo);
            var conversationParser = new ConversationParser(gameInfo);

            foreach (var script in gameInfo.RoomScripts)
            {
                var commands = roomParser.Parse(script.Value);

                dialogSet.Add(new Room(script.Key, commands, gameInfo, _stateAccessors.InventoryItemsAccessor,
                    _stateAccessors.StateFlagsAccessor, _stateAccessors.RoomStateAccessor));
            }

            foreach (var script in gameInfo.ConversationScripts)
            {
                var conversationRootNode = conversationParser.Parse(script.Value);

                dialogSet.Add(new Conversation(script.Key, gameInfo, conversationRootNode,
                    _stateAccessors.StateFlagsAccessor));
            }

            return dialogSet;
        }

        #region LUIS

        private async Task<string> DetermineCommandAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            var recognizerResult = await _services.LuisServices["gameatron4000"]
                .RecognizeAsync<LUISModel>(context, cancellationToken);

            // parse LUIS results to get intent and entity
            string intent = GetLUISIntent(recognizerResult);
            if (intent != null)
            {
                IEnumerable<string> entities = GetLUISEntities(recognizerResult);
                if (entities.Count() > 0)
                {
                    switch(intent)
                    {
                        case "use":
                            return $"use {entities.First()} with {entities.Last()}";

                        case "give":
                            return $"give {entities.First()} to {entities.Last()}";
                        
                        default:
                            return $"{intent} {entities.First()}";        
                    }
                }
            }

            return null;
        }

        private string GetLUISIntent(LUISModel luisResult)
        {
            string intent = null;
            var topIntent = luisResult.TopIntent();
            if (topIntent.intent  != LUISModel.Intent.None)
            {
                intent = topIntent.intent.ToString().Replace("_", " ");
            }
            return intent;
        }

        private IEnumerable<string> GetLUISEntities(LUISModel luisResult)
        {
            List<string> entities = new List<string>();

            if (luisResult.Entities.GameObject?.Count() > 0)
            {
                entities.AddRange(luisResult.Entities.GameObject.Select(o => o[0]).ToList());
            }
            if (luisResult.Entities.GameActor?.Count() > 0)
            {
                entities.AddRange(luisResult.Entities.GameActor.Select(o => o[0]).ToList());
            }

            return entities;
        }

        #endregion
    }
}