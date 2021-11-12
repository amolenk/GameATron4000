namespace Amolenk.GameATron4000.Model.Actions;

public class TalkToAction : UnaryAction
{
    public TalkToAction() : base(Verb.TalkTo)
    {
    }

    public override void Execute(GameObjectHandlers handlers) =>
        handlers.HandleTalkTo?.Invoke();
}