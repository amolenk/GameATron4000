namespace Amolenk.GameATron4000.Model.Builders;

public class GameObjectHandlersBuilder<TParent> : IGameObjectHandlersBuilder
{
    private readonly TParent _parentBuilder;

    public Action? HandleGive { get; private set; }
    public Action? HandlePickUp { get; private set; }
    public Action? HandleUse { get; private set; }
    public Action? HandleOpen { get; private set; }
    public Action? HandleLookAt { get; private set; }
    public Action? HandlePush { get; private set; }
    public Action? HandleClose { get; private set; }
    public Action? HandleTalkTo { get; private set; }
    public Action? HandlePull { get; private set; }


    internal GameObjectHandlersBuilder(TParent parentBuilder)
    {
        _parentBuilder = parentBuilder;
    }

    public TParent Give(Action action)
    {
        HandleGive = action;
        return _parentBuilder;
    }

    public TParent PickUp(Action action)
    {
        HandlePickUp = action;
        return _parentBuilder;
    }

    public TParent Use(Action action)
    {
        HandleUse = action;
        return _parentBuilder;
    }

    public TParent Open(Action action)
    {
        HandleOpen = action;
        return _parentBuilder;
    }

    public TParent LookAt(Action action)
    {
        HandleLookAt = action;
        return _parentBuilder;
    }

    public TParent Push(Action action)
    {
        HandlePush = action;
        return _parentBuilder;
    }

    public TParent Close(Action action)
    {
        HandleClose = action;
        return _parentBuilder;
    }

    public TParent TalkTo(Action action)
    {
        HandleTalkTo = action;
        return _parentBuilder;
    }

    public TParent Pull(Action action)
    {
        HandlePull = action;
        return _parentBuilder;
    }

    public void Build() => new GameObjectHandlers(this);
}
