namespace Amolenk.GameATron4000.Model.Actions;

public class PullAction : UnaryAction
{
    public PullAction() : base(Verb.Pull)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandlePull?.Invoke();
}