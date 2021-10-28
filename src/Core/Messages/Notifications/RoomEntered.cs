namespace Amolenk.GameATron4000.Messages.Notifications;

public record RoomEntered(Room Room, IEnumerable<ActorTemp> Actors)
    : INotification
{
}
