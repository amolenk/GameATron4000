namespace Amolenk.GameATron4000.Model.Actions;

public class TalkToAction : UnaryAction
{
    public TalkToAction(Game game) : base(game, Verb.TalkTo)
    {
    }

    public override Action? GetHandler(ActionHandlers actionHandlers) =>
        actionHandlers.HandleTalkTo;
}