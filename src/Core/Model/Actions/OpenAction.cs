namespace Amolenk.GameATron4000.Model.Actions;

public class OpenAction : UnaryAction
{
    public OpenAction(Game game) : base(game, Verb.Open, true)
    {
    }

    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandleOpen;
}