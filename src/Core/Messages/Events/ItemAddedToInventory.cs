namespace Amolenk.GameATron4000.Messages.Events;

public record ItemAddedToInventory(Item Item, Actor NewOwner) : IEvent;
