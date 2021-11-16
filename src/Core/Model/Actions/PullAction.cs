namespace Amolenk.GameATron4000.Model.Actions;

public class PullAction : UnaryAction
{
    public PullAction(Game game) : base(game, Verb.Pull)
    {
    }

    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandlePull;
}