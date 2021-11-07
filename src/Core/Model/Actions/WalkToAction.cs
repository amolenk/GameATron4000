namespace Amolenk.GameATron4000.Model.Actions;

public class WalkToAction : UnaryAction
{
    public WalkToAction() : base(Verb.WalkTo)
    {
    }

    public override void Execute(GameObjectHandlers handlers)
    {
        // No handlers for WalkTo command.
    }
}