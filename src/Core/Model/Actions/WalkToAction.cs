namespace Amolenk.GameATron4000.Model.Actions;

public class WalkToAction : UnaryAction
{
    public WalkToAction(Game game) : base(game, Verb.WalkTo)
    {
    }

    public override void TryExecute(ActionHandlers actionHandlers) =>
        actionHandlers.HandleWalkTo?.Invoke();
}