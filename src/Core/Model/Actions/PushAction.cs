namespace Amolenk.GameATron4000.Model.Actions;

public class PushAction : UnaryAction
{
    public PushAction() : base(Verb.Push)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandlePush?.Invoke();
}