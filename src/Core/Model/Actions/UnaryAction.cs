namespace Amolenk.GameATron4000.Model.Actions;

public abstract class UnaryAction : IAction
{
    private readonly Verb _verb;
    private GameObject? _subject;

    protected UnaryAction(Verb verb)
    {
        _verb = verb;
    }

    public bool Add(GameObject gameObject)
    {
        _subject = gameObject;
        return true;
    }

    public GameObject? GetInteractObject() => _subject;

    public string GetDisplayText(GameObject? gameObject)
    {
		var stringBuilder = new StringBuilder(_verb.Text);
		
		if (gameObject is not null)
		{
			stringBuilder.Append($" {gameObject.DisplayName}");
		}

		return stringBuilder.ToString();
    }

    public void Execute() => Execute(_subject!.Handlers);

    public abstract void Execute(GameObjectHandlers handlers);
}