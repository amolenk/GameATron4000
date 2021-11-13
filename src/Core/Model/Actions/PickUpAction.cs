namespace Amolenk.GameATron4000.Model.Actions;

public class PickUpAction : UnaryAction
{
    public PickUpAction(Game game) : base(game, Verb.PickUp)
    {
    }

    public override void TryExecute(ActionHandlers actionHandlers) =>
        actionHandlers.HandlePickUp?.Invoke();
}