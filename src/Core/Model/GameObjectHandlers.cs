namespace Amolenk.GameATron4000.Model.Actions;

public class GameObjectHandlers
{
    public Action<Actor>? HandleGive { get; private set; }
    public Action? HandlePickUp { get; private set; }
    public Action<GameObject?>? HandleUse { get; private set; }
    public Action? HandleOpen { get; private set; }
    public Action? HandleLookAt { get; private set; }
    public Action? HandlePush { get; private set; }
    public Action? HandleClose { get; private set; }
    public Action? HandleTalkTo { get; private set; }
    public Action? HandlePull { get; private set; }

    public GameObjectHandlers(IGameObjectHandlersBuilder builder)
    {
        HandleGive = builder.HandleGive;
        HandlePickUp = builder.HandlePickUp;
        HandleUse = builder.HandleUse;
        HandleOpen = builder.HandleOpen;
        HandleLookAt = builder.HandleLookAt;
        HandlePush = builder.HandlePush;
        HandleClose = builder.HandleClose;
        HandleTalkTo = builder.HandleTalkTo;
        HandlePull = builder.HandlePull;
    }
}
