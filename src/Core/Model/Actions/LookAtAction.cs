namespace Amolenk.GameATron4000.Model.Actions;

public class LookAtAction : UnaryAction
{
    public LookAtAction(Game game) : base(game, Verb.LookAt)
    {
    }

    public override void TryExecute(ActionHandlers actionHandlers) =>
        actionHandlers.HandleLookAt?.Invoke();
}