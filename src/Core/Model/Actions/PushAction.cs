namespace Amolenk.GameATron4000.Model.Actions;

public class PushAction : UnaryAction
{
    public PushAction(Game game) : base(game, Verb.Push)
    {
    }

    public override void TryExecute(ActionHandlers actionHandlers) =>
        actionHandlers.HandlePush?.Invoke();
}