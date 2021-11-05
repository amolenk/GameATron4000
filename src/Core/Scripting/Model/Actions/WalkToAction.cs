namespace Amolenk.GameATron4000.Scripting.Model.Actions;

public class WalkToAction : UnaryAction
{
    public WalkToAction() : base(Verb.WalkTo)
    {
    }

    public override void Execute(ActionHandlers handlers)
    {
        // No handlers for WalkTo command.
    }
}