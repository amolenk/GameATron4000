namespace Amolenk.GameATron4000.Model.Actions;

public class OpenAction : UnaryAction
{
    public OpenAction() : base(Verb.Open)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandleOpen?.Invoke();
}