namespace Amolenk.GameATron4000.Model.Actions;

public class PullAction : UnaryAction
{
    public PullAction(Game game) : base(game, Verb.Pull)
    {
    }

    public override void TryExecute(ActionHandlers actionHandlers) =>
        actionHandlers.HandlePull?.Invoke();
}