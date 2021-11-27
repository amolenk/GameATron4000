namespace Amolenk.GameATron4000.Model.Builders;

public class ActionHandlersBuilder
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

    public ActionHandlersBuilder Give(Action<Actor> action)
    {
        HandleGive = action;
        return this;
    }

    public ActionHandlersBuilder PickUp(Action action)
    {
        HandlePickUp = action;
        return this;
    }

    public ActionHandlersBuilder Use(Action<GameObject?> action)
    {
        HandleUse = action;
        return this;
    }

    public ActionHandlersBuilder Open(Action action)
    {
        HandleOpen = action;
        return this;
    }

    public ActionHandlersBuilder LookAt(Action action)
    {
        HandleLookAt = action;
        return this;
    }

    public ActionHandlersBuilder Push(Action action)
    {
        HandlePush = action;
        return this;
    }

    public ActionHandlersBuilder Close(Action action)
    {
        HandleClose = action;
        return this;
    }

    public ActionHandlersBuilder TalkTo(Action action)
    {
        HandleTalkTo = action;
        return this;
    }

    public ActionHandlersBuilder Pull(Action action)
    {
        HandlePull = action;
        return this;
    }

    public ActionHandlersBuilder WalkTo(Action action)
    {
        HandleWalkTo = action;
        return this;
    }

    internal ActionHandlers BuildActionHandlers() => new ActionHandlers(
        HandleGive,
        HandlePickUp,
        HandleUse,
        HandleOpen,
        HandleLookAt,
        HandlePush,
        HandleClose,
        HandleTalkTo,
        HandlePull,
        HandleWalkTo);
}
