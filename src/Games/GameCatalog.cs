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
            var roomsDir = Path.Combine(gameDir, "rooms");
            var roomParser = new RoomParser();
            var dialogTreeParser = new DialogTreeParser();

            var infoPath = Path.Combine(gameDir, "game.json");
            var infoJson = File.ReadAllText(infoPath);
            var info = JsonConvert.DeserializeObject<GameInfo>(infoJson);
            info.Dialogs = new DialogSet();

            foreach (var room in info.Rooms)
            {
                var roomDir = Path.Combine(roomsDir, room.Id);
                var commands = roomParser.Parse(Path.Combine(roomDir, "script.rm"));
                
                info.Dialogs.Add(Path.GetFileName(roomDir), new Room(room, commands));

                // foreach (var dialogTreePath in Directory.GetFiles(roomDir, "*.dt"))
                // {
                //     var rootNode = dialogTreeParser.Parse(dialogTreePath);

                //     info.Dialogs.Add(Path.GetFileNameWithoutExtension(dialogTreePath), new DialogTree(rootNode));
                // }
            }

            return info;
        }
    }
}