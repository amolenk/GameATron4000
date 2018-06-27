using System.Collections.Generic;
using System.Threading.Tasks;
using GameATron4000.Dialogs;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace GameATron4000
{
    public class GameBot : IBot
    {
        private readonly DialogSet _dialogs;

        public GameBot()
        {
            var scriptParser = new ScriptParser();
            var commands = scriptParser.Parse("script.game");

            _dialogs = new DialogSet();
            _dialogs.Add("ParkRoom", new Room(commands));
            _dialogs.Add("talk_to_al", new ActorConversation());
        }

        public async Task OnTurn(ITurnContext context)
        {
            var state = ConversationState<Dictionary<string, object>>.Get(context);
            var dc = _dialogs.CreateContext(context, state);

            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                await dc.Continue();

                if (!context.Responded)
                {
                    await dc.Begin("ParkRoom");
                }
            }
        }
    }
}