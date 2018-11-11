using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameATron4000.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public GameInfo GetGameInfo(string gameName)
        {
            var gameDir = Path.Combine(_baseDir, gameName);
            var scriptDir = Path.Combine(gameDir, "scripts");

            var gameInfoPath = Path.Combine(gameDir, "game.json");
            var gameInfoJson = File.ReadAllText(gameInfoPath);
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(gameInfoJson);

            // Update the paths of the loaded scripts to absolute paths.
            foreach (var script in gameInfo.RoomScripts.Concat(gameInfo.ConversationScripts))
            {
                script.Path = Path.Combine(scriptDir, script.Path);
            }

            return gameInfo;
        }
    }
}