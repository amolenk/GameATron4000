using System.IO;
using GameATron4000.Dialogs;
using GameATron4000.Games;
using GameATron4000.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace GameATron4000.Dialogs
{
    public class GameDialogSet : DialogSet
    {
        public GameDialogSet(GameInfo gameInfo, GameFlags gameFlags, IStatePropertyAccessor<DialogState> dialogStateAccessor)
            : base(dialogStateAccessor)
        {
            var roomParser = new RoomParser();
            var conversationParser = new ConversationParser();

            foreach (var script in gameInfo.RoomScripts)
            {
                var commands = roomParser.Parse(script.Path);

                Add(new Room(script.Key, commands, gameInfo, gameFlags));
            }

            foreach (var script in gameInfo.ConversationScripts)
            {
                var conversationRootNode = conversationParser.Parse(script.Path);

                Add(new Conversation(script.Key, conversationRootNode, gameFlags));
            }
        }
    }
}