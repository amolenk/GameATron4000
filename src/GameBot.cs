using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Configuration;
using GameATron4000.Dialogs;
using GameATron4000.Games;
using GameATron4000.Models;
using GameATron4000.Models.Actions;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameATron4000
{
    public class GameBot : IBot
    {
        private readonly BotServices _services;
        private readonly GameBotAccessors _stateAccessors;
        private readonly LUISOptions _luisOptions;
        private readonly GameCatalog _gameCatalog;

        public GameBot(BotServices services, GameBotAccessors stateAccessors, IOptions<LUISOptions> luisOptionsAccessor, GameCatalog gameCatalog)
        {
            _services = services;
            _stateAccessors = stateAccessors;
            _luisOptions = luisOptionsAccessor.Value;
            _gameCatalog = gameCatalog;
        }

        public async Task OnTurnAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            if (context.Activity.Type is ActivityTypes.ConversationUpdate)
            {
                foreach (var newMember in context.Activity?.MembersAdded)
                {
                    if (newMember.Id == context.Activity.Recipient.Id)
                    {
                        var gameList = _gameCatalog.GetGameNames().ToList();
                        await context.SendActivityAsync(MessageFactory.SuggestedActions(gameList, "Which game do you want to play?"));
                        return;
                    }
                }
            }
            else if (context.Activity.Type is ActivityTypes.Message)
            {
                // Load the current game state from conversation state.
                // If no game state is found, start a new game from the user's input.
                var gameState = await _stateAccessors.GameStateAccessor.GetAsync(context, () => new GameState
                {
                    GameName = context.Activity.Text,
                    GameFlags = new GameFlags()
                });

                // Load the metadata for the selected game.
                var gameInfo = _gameCatalog.GetGameInfo(gameState.GameName);

                // Establish dialog context from the loaded game.
                var dialogSet = new GameDialogSet(gameInfo, gameState.GameFlags, _stateAccessors.DialogStateAccessor);
                var dc = await dialogSet.CreateContextAsync(context, cancellationToken);

                if (dc.ActiveDialog == null)
                {
                    // Start the game's first room.
                    var rootDialog = gameInfo.InitialRoom;
                    await dc.BeginDialogAsync(rootDialog);
                }
                else
                {
                    // get intent and entity from LUIS (if enabled).
                    if (_luisOptions.Enabled)
                    {
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
        }

        private async Task<string> DetermineCommandAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            var recognizerResult = await _services.LuisServices["gameatron4000"]
                .RecognizeAsync<LUISModel>(context, cancellationToken);

            // parse LUIS results to get intent and entity
            string intent = GetLUISIntent(recognizerResult);
            if (intent != null)
            {
                string entity = GetLUISEntity(recognizerResult);
                if (entity != null)
                {
                    return $"{intent} {entity}";
                }
            }

            return null;
        }

        #region LUIS result parsing

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

        private string GetLUISEntity(LUISModel luisResult)
        {
            string entity = null;

            double largestScore = 0;
            if (luisResult.Entities._instance.Al != null)
            {
                var entityHit = luisResult.Entities._instance.Al.FirstOrDefault(id => id.Score > _luisOptions.ScoreThreshold);
                if (entityHit != null)
                {
                    largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                    entity = "al";
                }
            }
            if (luisResult.Entities.Guy_Scotthrie != null)
            {
                var entityHit = luisResult.Entities._instance.Guy_Scotthrie.FirstOrDefault(id => id.Score > _luisOptions.ScoreThreshold);
                if (entityHit != null)
                {
                    largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                    entity = "guy scotthrie";
                }
            }
            if (luisResult.Entities.Ian != null)
            {
                var entityHit = luisResult.Entities._instance.Ian.FirstOrDefault(id => id.Score > _luisOptions.ScoreThreshold);
                if (entityHit != null)
                {
                    largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                    entity = "ian";
                }
            }
            if (luisResult.Entities.newspaper != null)
            {
                var entityHit = luisResult.Entities._instance.newspaper.FirstOrDefault(id => id.Score > _luisOptions.ScoreThreshold);
                if (entityHit != null)
                {
                    largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                    entity = "newspaper";
                }
            }

            return entity;
        }

        #endregion
    }
}