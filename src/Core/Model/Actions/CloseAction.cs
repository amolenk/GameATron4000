namespace Amolenk.GameATron4000.Model.Actions;

public class CloseAction : UnaryAction
{
    public CloseAction(Game game) : base(game, Verb.Close, true)
    {
    }

    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandleClose;
}