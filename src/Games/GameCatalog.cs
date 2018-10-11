using System.Collections.Generic;
using System.IO;
using GameATron4000.Dialogs;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Games
{
    public class GameCatalog
    {
        private readonly string _baseDir;

        public GameCatalog(string baseDir)
        {
            _baseDir = baseDir;
        }

        public IEnumerable<string> GetGameNames()
        {
            foreach (var gameDir in Directory.GetDirectories(_baseDir))
            {
                yield return Path.GetFileName(gameDir);
            }
        }

        public GameInfo LoadGame(string name)
        {
            var gameDir = Path.Combine(_baseDir, name);
            var roomParser = new RoomParser();
            var conversationParser = new ConversationParser();

            var gameInfoPath = Path.Combine(gameDir, "game.json");
            var gameInfoJson = File.ReadAllText(gameInfoPath);
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(gameInfoJson);
            gameInfo.Dialogs = new DialogSet();

            var scriptDir = Path.Combine(gameDir, "scripts");

            foreach (var roomScriptPath in Directory.GetFiles(scriptDir, "*.room"))
            {
                var roomId = Path.GetFileNameWithoutExtension(roomScriptPath);
                var commands = roomParser.Parse(roomScriptPath);

                gameInfo.Dialogs.Add(roomId, new Room(roomId, commands));
            }

            foreach (var conversationScriptPath in Directory.GetFiles(scriptDir, "*.conversation"))
            {
                var conversationId = Path.GetFileNameWithoutExtension(conversationScriptPath);
                var conversationRootNode = conversationParser.Parse(conversationScriptPath);

                gameInfo.Dialogs.Add(conversationId, new Conversation(conversationRootNode));
            }

            return gameInfo;
        }
    }
}