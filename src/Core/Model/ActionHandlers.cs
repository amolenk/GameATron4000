namespace Amolenk.GameATron4000.Model;

public class ActionHandlers
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
    public Action? HandleWalkTo { get; private set; }

    public ActionHandlers()
    {
    }

    // TODO Remove
    public ActionHandlers(
        Action<Actor>? handleGive,
        Action? handlePickUp,
        Action<GameObject?>? handleUse,
        Action? handleOpen,
        Action? handleLookAt,
        Action? handlePush,
        Action? handleClose,
        Action? handleTalkTo,
        Action? handlePull,
        Action? handleWalkTo)
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
        HandleWalkTo = handleWalkTo;
    }
    public void Give(Action<Actor> give)
    {
        HandleGive = give;
    }

    public void PickUp(Action pickUp)
    {
        HandlePickUp = pickUp;
    }

    public void Use(Action<GameObject?> use)
    {
        HandleUse = use;
    }

    public void Open(Action open)
    {
        HandleOpen = open;
    }

    public void LookAt(Action lookAt)
    {
        HandleLookAt = lookAt;
    }

    public void Push(Action push)
    {
        HandlePush = push;
    }

    public void Close(Action close)
    {
        HandleClose = close;
    }

    public void TalkTo(Action talkTo)
    {
        HandleTalkTo = talkTo;
    }

    public void Pull(Action pull)
    {
        HandlePull = pull;
    }

    public void WalkTo(Action walkTo)
    {
        HandleWalkTo = walkTo;
    }
}
