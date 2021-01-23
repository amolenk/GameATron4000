using System;
using GameATron4000.Core.Services;

namespace GameATron4000.Core.Messages
{
    public class InventoryItemRemovedEvent
    {
        public InventoryItemRemovedEvent(IItemState item)
        {
            Item = item;
        }

        public IItemState Item;
    }
}
