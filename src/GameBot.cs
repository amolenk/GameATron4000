using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GameATron4000.Dialogs;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using src.Models;

namespace GameATron4000
{
    public class GameBot : IBot
    {
        private readonly GameInfo _game;

        public GameBot()
        {
            _game = LoadGame("ReturnOfTheBodySnatchers");
        }

        public async Task OnTurn(ITurnContext context)
        {
            var state = ConversationState<Dictionary<string, object>>.Get(context);
            var dc = _game.Dialogs.CreateContext(context, state);

            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                await dc.Continue();

                if (!context.Responded)
                {
                    var rootDialog = _game.InitialRoom;

                    await dc.Begin(rootDialog);
                }
            }
        }

        private GameInfo LoadGame(string name)
        {
            var gameDir = Path.Combine("Games", name);
            var scriptParser = new ScriptParser();
            var conversationScriptParser = new ConversationScriptParser();

            var infoPath = Path.Combine(gameDir, "game.json");
            var infoJson = File.ReadAllText(infoPath);
            var info = JsonConvert.DeserializeObject<GameInfo>(infoJson);
            info.Dialogs = new DialogSet();

            foreach (var roomDir in Directory.GetDirectories(Path.Combine(gameDir, "rooms")))
            {
                var commands = scriptParser.Parse(Path.Combine(roomDir, "script.room"));

                info.Dialogs.Add(Path.GetFileName(roomDir), new Room(commands));

                foreach (var conversationPath in Directory.GetFiles(roomDir, "*.conversation"))
                {
                    var result = conversationScriptParser.Parse(conversationPath);

                    info.Dialogs.Add(Path.GetFileNameWithoutExtension(conversationPath), new Conversation());
                }
            }

            return info;
        }
    }
}