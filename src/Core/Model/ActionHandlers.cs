namespace Amolenk.GameATron4000.Model;

public class ActionHandlers
{
    public Action<Actor>? HandleGive { get; private set; }
    public Action? HandlePickUp { get; private set; }
    public Action<IGameObject?>? HandleUse { get; private set; }
    public Action? HandleOpen { get; private set; }
    public Action? HandleLookAt { get; private set; }
    public Action? HandlePush { get; private set; }
    public Action? HandleClose { get; private set; }
    public Action? HandleTalkTo { get; private set; }
    public Action? HandlePull { get; private set; }

    public ActionHandlers(
        Action<Actor>? handleGive,
        Action? handlePickUp,
        Action<IGameObject?>? handleUse,
        Action? handleOpen,
        Action? handleLookAt,
        Action? handlePush,
        Action? handleClose,
        Action? handleTalkTo,
        Action? handlePull)
    {
        HandleGive = handleGive;
        HandlePickUp = handlePickUp;
        HandleUse = handleUse;
        HandleOpen = handleOpen;
        HandleLookAt = handleLookAt;
        HandlePush = handlePush;
        HandleClose = handleClose;
        HandleTalkTo = handleTalkTo;
        HandlePull = handlePull;
    }
}
