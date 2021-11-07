namespace Amolenk.GameATron4000.Model.Builders;

public interface IGameObjectHandlersBuilder
{
     Action? HandleGive { get; }
     Action? HandlePickUp { get; }
     Action? HandleUse { get; }
     Action? HandleOpen { get; }
     Action? HandleLookAt { get; }
     Action? HandlePush { get; }
     Action? HandleClose { get; }
     Action? HandleTalkTo { get; }
     Action? HandlePull { get; }
}
