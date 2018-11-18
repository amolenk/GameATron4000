namespace GameATron4000.Models
{
    public class CommandActionResult
    {
        public static readonly CommandActionResult None = new CommandActionResult(DialogType.None, string.Empty);

        private CommandActionResult(DialogType dialogType, string dialogId)
        {
            NextDialogType = dialogType;
            NextDialogId = dialogId;
        }

        public DialogType? NextDialogType { get; private set; }

        public string NextDialogId { get; private set; }

        public static CommandActionResult StartConversation(string conversationId)
        {
            return new CommandActionResult(DialogType.Conversation, conversationId);
        }

        public static CommandActionResult SwitchRoom(string roomId)
        {
            return new CommandActionResult(DialogType.Room, roomId);
        }
    }
}