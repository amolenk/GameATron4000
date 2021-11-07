namespace Amolenk.GameATron4000.Model.Actions;

public class LookAtAction : UnaryAction
{
    public LookAtAction() : base(Verb.LookAt)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandleLookAt?.Invoke();
}