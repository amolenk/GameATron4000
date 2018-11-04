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
        private readonly GameBotAccessors _stateAccessors;

        public GameCatalog(string baseDir, GameBotAccessors stateAccessors)
        {
            _baseDir = baseDir;
            _stateAccessors = stateAccessors;
        }

        public static IEnumerable<string> GetGameNames(string baseDir)
        {
            foreach (var gameDir in Directory.GetDirectories(baseDir))
            {
                yield return Path.GetFileName(gameDir);
            }
        }

        public IEnumerable<string> GetGameNames()
        {
            return GameCatalog.GetGameNames(_baseDir);
        }

        public GameInfo LoadGame(string name)
        {
            var gameDir = Path.Combine(_baseDir, name);
            var roomParser = new RoomParser();
            var conversationParser = new ConversationParser();

            var gameInfoPath = Path.Combine(gameDir, "game.json");
            var gameInfoJson = File.ReadAllText(gameInfoPath);
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(gameInfoJson);
            gameInfo.Dialogs = new DialogSet(_stateAccessors.DialogState);

            var scriptDir = Path.Combine(gameDir, "scripts");

            foreach (var roomScriptPath in Directory.GetFiles(scriptDir, "*.room"))
            {
                var roomId = Path.GetFileNameWithoutExtension(roomScriptPath);
                var commands = roomParser.Parse(roomScriptPath);

                gameInfo.Dialogs.Add(new Room(roomId, commands, gameInfo.BadCommandResponses, 
                    gameInfo.PlayerActor, _stateAccessors));
            }

            foreach (var conversationScriptPath in Directory.GetFiles(scriptDir, "*.conversation"))
            {
                var conversationId = Path.GetFileNameWithoutExtension(conversationScriptPath);
                var conversationRootNode = conversationParser.Parse(conversationScriptPath);

                gameInfo.Dialogs.Add(new Conversation(conversationId, conversationRootNode, _stateAccessors));
            }

            return gameInfo;
        }
    }
}