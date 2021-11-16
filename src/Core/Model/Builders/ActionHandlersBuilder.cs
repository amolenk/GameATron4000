namespace Amolenk.GameATron4000.Model.Builders;

public class ActionHandlersBuilder<TParentBuilder>
{
    private readonly TParentBuilder _parentBuilder;

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

    internal ActionHandlersBuilder(TParentBuilder parentBuilder)
    {
        _parentBuilder = parentBuilder;
    }

    public TParentBuilder Give(Action<Actor> action)
    {
        HandleGive = action;
        return _parentBuilder;
    }

    public TParentBuilder PickUp(Action action)
    {
        HandlePickUp = action;
        return _parentBuilder;
    }

    public TParentBuilder Use(Action<GameObject?> action)
    {
        HandleUse = action;
        return _parentBuilder;
    }

    public TParentBuilder Open(Action action)
    {
        HandleOpen = action;
        return _parentBuilder;
    }

    public TParentBuilder LookAt(Action action)
    {
        HandleLookAt = action;
        return _parentBuilder;
    }

    public TParentBuilder Push(Action action)
    {
        HandlePush = action;
        return _parentBuilder;
    }

    public TParentBuilder Close(Action action)
    {
        HandleClose = action;
        return _parentBuilder;
    }

    public TParentBuilder TalkTo(Action action)
    {
        HandleTalkTo = action;
        return _parentBuilder;
    }

    public TParentBuilder Pull(Action action)
    {
        HandlePull = action;
        return _parentBuilder;
    }

    public TParentBuilder WalkTo(Action action)
    {
        HandleWalkTo = action;
        return _parentBuilder;
    }
}
