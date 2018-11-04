using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace GameATron4000
{
     /// <summary>
    /// This class is created as a Singleton and passed into the IBot-derived constructor.
    ///  - See <see cref="GameBot"/> constructor for how that is injected.
    ///  - See the Startup.cs file for more details on creating the Singleton that gets
    ///    injected into the constructor.
    /// </summary>
    public class GameBotAccessors
    {
        public GameBotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public static string GameStateName { get; } = $"{nameof(GameBotAccessors)}.GameState";


        public static string DialogStateAccessorName { get; } = $"{nameof(GameBotAccessors)}.DialogState";

        public IStatePropertyAccessor<DialogState> DialogState { get; set; }

        public IStatePropertyAccessor<IDictionary<string,object>> GameState { get; set; }

        public ConversationState ConversationState { get; }
    }
}