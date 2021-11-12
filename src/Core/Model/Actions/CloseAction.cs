namespace Amolenk.GameATron4000.Model.Actions;

public class CloseAction : UnaryAction
{
    public CloseAction() : base(Verb.Close)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandleClose?.Invoke();
}