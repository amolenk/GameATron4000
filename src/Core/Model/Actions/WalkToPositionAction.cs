namespace Amolenk.GameATron4000.Model.Actions;

public class WalkToPositionAction : IAction
{
    private readonly Actor _actor;
    private readonly Point _to;

    public bool DisableUIWhileExecuting => false;

    public WalkToPositionAction(Actor actor, Point to)
    {
        _actor = actor;
        _to = to;
    }

    public bool Add(GameObject gameObject) => false;

    public GameObject? GetObjectToMoveTo() => null;

    public string GetDisplayText(GameObject? overObject = null) =>
        $"Walk to {_to}";

    public bool TryExecute()
    {
        _actor.MoveTo(_to);
        return true;
    }
}