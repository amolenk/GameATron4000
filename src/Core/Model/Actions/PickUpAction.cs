namespace Amolenk.GameATron4000.Model.Actions;

public class PickUpAction : UnaryAction
{
    public PickUpAction(Game game) : base(game, Verb.PickUp)
    {
    }

    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandlePickUp;
}