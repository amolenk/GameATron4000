using System.Collections.Generic;
using System.IO;
using GameATron4000.Dialogs;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000
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
            var dialogTreeParser = new DialogTreeParser();

            var infoPath = Path.Combine(gameDir, "game.json");
            var infoJson = File.ReadAllText(infoPath);
            var info = JsonConvert.DeserializeObject<GameInfo>(infoJson);
            info.Dialogs = new DialogSet();

            foreach (var roomDir in Directory.GetDirectories(Path.Combine(gameDir, "rooms")))
            {
                var commands = roomParser.Parse(Path.Combine(roomDir, "script.rm"));

                info.Dialogs.Add(Path.GetFileName(roomDir), new Room(commands));

                foreach (var dialogTreePath in Directory.GetFiles(roomDir, "*.dt"))
                {
                    var rootNode = dialogTreeParser.Parse(dialogTreePath);

                    info.Dialogs.Add(Path.GetFileNameWithoutExtension(dialogTreePath), new DialogTree(rootNode));
                }
            }

            return info;
        }
    }
}