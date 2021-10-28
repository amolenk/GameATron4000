namespace Amolenk.GameATron4000.Engine.Messages.Notifications;

public record RoomEnteredEvent(
    Room Room,
    IEnumerable<ActorTemp> Actors)
    : INotification
{
}
