namespace Amolenk.GameATron4000.Model.Actions;

public class PickUpAction : UnaryAction
{
    public PickUpAction() : base(Verb.PickUp)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandlePickUp?.Invoke();
}