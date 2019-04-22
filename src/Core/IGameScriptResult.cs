using System.Collections.Generic;
using Microsoft.Bot.Schema;

namespace GameATron4000.Core
{
    public interface IGameScriptResult
    {
        List<Activity> Activities { get; }

        Dictionary<string, string> ConversationOptions { get; }

        string NextDialogId { get; }

        bool NextDialogReplace { get; }
    }
}