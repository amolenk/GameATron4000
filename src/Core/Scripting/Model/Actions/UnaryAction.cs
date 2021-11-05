namespace Amolenk.GameATron4000.Scripting.Model.Actions;

public abstract class UnaryAction : IAction
{
    public Verb Verb { get; }
    public GameObject? Subject { get; private set; }

    public UnaryAction(Verb verb)
    {
        Verb = verb;
    }

    public bool Add(GameObject gameObject)
    {
        Subject = gameObject;
        return true;
    }

    public string GetDisplayText(GameObject? gameObject)
    {
		var stringBuilder = new StringBuilder(Verb.Text);
		
		if (gameObject != null)
		{
			stringBuilder.Append($" {gameObject.Id}");
		}

		return stringBuilder.ToString();
    }

    public abstract void Execute(ActionHandlers handlers);
}