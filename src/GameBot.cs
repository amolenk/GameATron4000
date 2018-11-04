using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Dialogs;
using GameATron4000.Games;
using GameATron4000.Models;
using GameATron4000.Models.Actions;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace GameATron4000
{
    public class GameBot : IBot
    {
        private readonly GameCatalog _gameCatalog;
        private readonly GameBotAccessors _stateAccessors;
        private readonly BotServices _services;
        private const double LUIS_SCORE_THRESHOLD = 0.85;

        public GameBot(BotServices services, GameBotAccessors stateAccessors)
        {
            _services = services;
            _stateAccessors = stateAccessors;
            _gameCatalog = new GameCatalog("Games", stateAccessors);
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
                // get intent and entity from LUIS
                string luisResult = await GetLUISIntentAsync(context, cancellationToken);
                if (!string.IsNullOrEmpty(luisResult))
                {
                    context.Activity.Text = luisResult;
                }

                // Get the conversation state from the turn context
                var state = await _stateAccessors.GameState.GetAsync(context,
                    () => new Dictionary<string, object>(), cancellationToken);

                if (!state.ContainsKey("GameName"))
                {
                    state.Add("GameName", context.Activity.Text);
                    await _stateAccessors.GameState.SetAsync(context, state);
                }

                var game = _gameCatalog.LoadGame(state["GameName"].ToString());

                // Establish dialog context from the conversation state.
                var dc = await game.Dialogs.CreateContextAsync(context, cancellationToken);

                if (dc.ActiveDialog == null)
                {
                    // Start the game's first room.
                    var rootDialog = game.InitialRoom;
                    await dc.BeginDialogAsync(rootDialog);
                }
                else
                {
                    await dc.ContinueDialogAsync();
                }

                await _stateAccessors.ConversationState.SaveChangesAsync(context, true, cancellationToken);
            }
        }

        private async Task<string> GetLUISIntentAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            var recognizerResult = await _services.LuisServices["gameatron4000"].RecognizeAsync<LUISModel>(context, cancellationToken);
            var topIntent = recognizerResult.TopIntent();
            if (topIntent.intent  != LUISModel.Intent.None)
            {
                string intent = topIntent.intent.ToString().Replace("_", " ");
                string entity = null;

                double largestScore = 0;
                if (recognizerResult.Entities._instance.Al != null)
                {
                    var entityHit = recognizerResult.Entities._instance.Al.FirstOrDefault(id => id.Score > LUIS_SCORE_THRESHOLD);
                    if (entityHit != null)
                    {
                        largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                        entity = "al";
                    }
                }
                if (recognizerResult.Entities.Guy_Scotthrie != null)
                {
                    var entityHit = recognizerResult.Entities._instance.Guy_Scotthrie.FirstOrDefault(id => id.Score > LUIS_SCORE_THRESHOLD);
                    if (entityHit != null)
                    {
                        largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                        entity = "guy scotthrie";
                    }
                }
                if (recognizerResult.Entities.Ian != null)
                {
                    var entityHit = recognizerResult.Entities._instance.Ian.FirstOrDefault(id => id.Score > LUIS_SCORE_THRESHOLD);
                    if (entityHit != null)
                    {
                        largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                        entity = "ian";
                    }
                }
                if (recognizerResult.Entities.newspaper != null)
                {
                    var entityHit = recognizerResult.Entities._instance.newspaper.FirstOrDefault(id => id.Score > LUIS_SCORE_THRESHOLD);
                    if (entityHit != null)
                    {
                        largestScore = entityHit.Score.Value > largestScore ? entityHit.Score.Value : largestScore;
                        entity = "newspaper";
                    }
                }
                
                if (entity != null)
                {
                    return $"{intent} {entity}";
                }
            }
            return null;
        }
    }
}