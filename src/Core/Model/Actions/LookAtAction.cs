namespace Amolenk.GameATron4000.Model.Actions;

public class LookAtAction : UnaryAction
{
    public LookAtAction(Game game) : base(game, Verb.LookAt, true)
    {
    }
    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandleLookAt;
}