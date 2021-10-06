namespace Amolenk.GameATron4000.Engine.Messages.Notifications;

public record GameStartedEvent(GameManifest Manifest) : INotification
{
}
