namespace Amolenk.GameATron4000.Model.Actions;

public class WalkToObjectAction : UnaryAction
{
    public WalkToObjectAction(Game game) : base(game, Verb.WalkTo)
    {
    }

    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandleWalkTo;
}