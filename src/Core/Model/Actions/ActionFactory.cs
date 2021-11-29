namespace Amolenk.GameATron4000.Model.Actions;

public class ActionFactory
{
    private readonly Game _game;

    public ActionFactory(Game game)
    {
        _game = game;
    }

    public IAction Create(Verb verb)
    {
        if (verb == Verb.Give) return new GiveAction(_game);
        if (verb == Verb.PickUp) return new PickUpAction(_game);
        if (verb == Verb.Use) return new UseAction(_game);
        if (verb == Verb.Open) return new OpenAction(_game);
        if (verb == Verb.LookAt) return new LookAtAction(_game);
        if (verb == Verb.Push) return new PushAction(_game);
        if (verb == Verb.Close) return new CloseAction(_game);
        if (verb == Verb.TalkTo) return new TalkToAction(_game);
        if (verb == Verb.Pull) return new PullAction(_game);
        if (verb == Verb.WalkTo) return new WalkToObjectAction(_game);

        throw new ArgumentException(
            $"Cannot create action for unsupported verb '{verb}'.",
            nameof(verb));
    }
}
