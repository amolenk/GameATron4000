namespace Amolenk.GameATron4000.Engine.Messages.Notifications;

public record RoomEnteredEvent(
    string RoomName,
    IEnumerable<ActorTemp> Actors)
    : INotification
{
}
