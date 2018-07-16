using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Dialogs;
using GameATron4000.Models;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace GameATron4000
{
    public class GameBot : IBot
    {
        private const string MainMenuId = "mainMenu";

        private readonly GameCatalog _gameCatalog;
        private GameInfo _loadedGame;

        public GameBot()
        {
            _gameCatalog = new GameCatalog("Games");
        }

        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type is ActivityTypes.ConversationUpdate)
            {
                foreach (var newMember in context.Activity?.MembersAdded)
                {
                    if (newMember.Id != context.Activity.Recipient.Id)
                    {
                        var gameList = _gameCatalog.GetGameNames().ToList();
                        await context.SendActivity(MessageFactory.SuggestedActions(gameList, "Which game do you want to play?"));
                        return;
                    }
                }
            }
            else if (context.Activity.Type is ActivityTypes.Message)
            {
                // Get the conversation state from the turn context
                var state = ConversationState<Dictionary<string, object>>.Get(context);

                if (!state.ContainsKey("GameName"))
                {
                    // TODO Validate
                    state.Add("GameName", context.Activity.Text);
                }

                var game = _gameCatalog.LoadGame(state["GameName"].ToString());

                var dialogState = ConversationState<Dictionary<string, object>>.Get(context);

                // Establish dialog context from the conversation state.
                var dc = game.Dialogs.CreateContext(context, dialogState);

                if (dc.ActiveDialog == null)
                {
                    // Start the game's first room.
                    var rootDialog = game.InitialRoom;
                    await dc.Begin(rootDialog);
                }
                else
                {
                    await dc.Continue();
                }

                // Continue any current dialog.
                // await dc.Continue();

                // Every turn sends a response, so if no response was sent,
                // then there is no dialog currently active.
                // TODO Find a better way to check this. Maybe check the active dialog?
                // if (!context.Responded)
                // {
                //     // Start the game's first room.
                //     var rootDialog = game.InitialRoom;
                //     await dc.Begin(rootDialog);
                // }
            }
        }
    }
}