namespace Amolenk.GameATron4000.Engine.Messages.Notifications;

public record GameManifestLoadedEvent(GameManifest Manifest) : INotification;
