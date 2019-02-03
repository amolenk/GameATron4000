using System;
using System.Collections.Generic;
using GameATron4000.Models;
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
        public static readonly string DialogStateAccessorName = $"{nameof(GameBotAccessors)}.{nameof(DialogStateAccessor)}";

        public static readonly string StateFlagsAccessorName = $"{nameof(GameBotAccessors)}.{nameof(StateFlagsAccessor)}";

        public static readonly string InventoryItemsAccessorName = $"{nameof(GameBotAccessors)}.{nameof(InventoryItemsAccessor)}";

        public static readonly string RoomStateAccessorName = $"{nameof(GameBotAccessors)}.{nameof(RoomStateAccessor)}";

        public ConversationState ConversationState { get; }

        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

        public IStatePropertyAccessor<List<string>> StateFlagsAccessor { get; set; }

        public IStatePropertyAccessor<List<string>> InventoryItemsAccessor { get; set; }

        public IStatePropertyAccessor<Dictionary<string, RoomState>> RoomStateAccessor { get; set; }

        public GameBotAccessors(ConversationState conversationState)
        {
            this.ConversationState = conversationState
                ?? throw new ArgumentNullException(nameof(conversationState));
        }
    }
}