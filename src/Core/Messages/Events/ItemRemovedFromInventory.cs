namespace Amolenk.GameATron4000.Messages.Events;

public record ItemRemovedFromInventory(Item Item, Actor PreviousOwner) : IEvent;
