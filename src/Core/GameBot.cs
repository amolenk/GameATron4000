using System;
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
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLua;

namespace GameATron4000.Core
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
            IGameScriptState scriptState = await _stateAccessors.GameScriptStateAccessor.GetAsync(
                context, () => new LuaGameScriptState());

            var activityFactory = new ActivityFactory(context);

            // TODO Move inside try
            IGameScript script = new LuaGameScript(activityFactory);
            script.LoadScriptState(scriptState);

            try
            {

                if (context.Activity.Properties != null
                    && context.Activity.Properties.TryGetValue("player_pos", out JToken playerPosition))
                {
                    script.UpdatePlayerPosition(
                        (int)playerPosition.Value<double>("x"),
                        (int)playerPosition.Value<double>("y"));
                }
                
                // Establish dialog context from the game info.
                var dialogSet = CreateDialogSet(script);
                var dc = await dialogSet.CreateContextAsync(context, cancellationToken);

                if (dc.ActiveDialog == null)
                {
                    if (context.Activity.Type is ActivityTypes.ConversationUpdate)
                    {
                        foreach (var newMember in context.Activity?.MembersAdded)
                        {
                            if (newMember.Id == context.Activity.Recipient.Id)
                            {
                                var result = script.OnInitializeGame();
                                if (string.IsNullOrWhiteSpace(result.NextDialogId))
                                {
                                    // TODO
                                    throw new Exception("No room specified!");
                                }

                                // And send a GameStarted to the client that contains information on the selected
                                // actor, inventory items and camera.
                                await context.SendActivityAsync(activityFactory.GameStarted(script));

                                // Start the game's first room.
                                await dc.BeginDialogAsync(result.NextDialogId);
                            }
                        }
                    }
                }
                else if (context.Activity.Type is ActivityTypes.Message)
                {
                    if (context.Activity.Text == "__reset")
                    {
                        await context.SendActivityAsync(activityFactory.GameStarted(script));
                    }
                    else
                    {
                        // TODO Trial 3: Add call to LUIS service

                        // Continue the dialog to handle the incoming command.
                        await dc.ContinueDialogAsync();
                    }
                }

                scriptState = script.SaveScriptState();

                await _stateAccessors.GameScriptStateAccessor.SetAsync(dc.Context, scriptState);

                // Save any changes back to conversation state.
                await _stateAccessors.ConversationState.SaveChangesAsync(context, false, cancellationToken);
            }
            catch (Exception ex)
            {
                var debug = (LuaTable)((LuaGameScript)script).DebugStuff;
                var ian = (LuaTable)debug["ian"];
                await context.SendActivityAsync(activityFactory.ErrorOccured(ex));
            }
        }

        private DialogSet CreateDialogSet(IGameScript gameScript)
        {
            var dialogSet = new DialogSet(_stateAccessors.DialogStateAccessor);

            foreach (var room in gameScript.Rooms)
            {
                var dialog = new RoomDialog(room.Id, gameScript);

                dialogSet.Add(dialog);
            }

            // TODO Replace with gameScript.Conversations
            foreach (var path in Directory.GetFiles("Gameplay/scripts/conversations", "*.lua"))
            {
                var dialog = new ConversationDialog(Path.GetFileNameWithoutExtension(path), gameScript);

                dialogSet.Add(dialog);
            }

            return dialogSet;
        }
    }
}