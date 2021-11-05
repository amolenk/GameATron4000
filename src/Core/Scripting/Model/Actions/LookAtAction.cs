namespace Amolenk.GameATron4000.Scripting.Model.Actions;

public class LookAtAction : UnaryAction
{
    public LookAtAction() : base(Verb.LookAt)
    {
    }

    public override void Execute(ActionHandlers handlers) =>
        handlers.HandleLookAt?.Invoke();
}