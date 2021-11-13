namespace Amolenk.GameATron4000.Model.Actions;

// TODO Fix Walk to <inventory item>
public abstract class UnaryAction : IAction
{
    private readonly Game _game;
    private readonly Verb _verb;
    private GameObject? _gameObject;

    protected UnaryAction(Game game, Verb verb)
    {
        _game = game;
        _verb = verb;
    }

    public bool Add(GameObject gameObject)
    {
        _gameObject = gameObject;
        return true;
    }

    public GameObject? GetObjectToWalkTo()
    {
        if (_gameObject is Actor || (_gameObject is Item item &&
            !_game.TryGetOwnerForItem(item, out Actor _)))
        {
            return _gameObject;
        }

        return null;
    }

    public string GetDisplayText(GameObject? gameObject)
    {
		var stringBuilder = new StringBuilder(_verb.Text);
		
		if (gameObject is not null)
		{
			stringBuilder.Append($" {gameObject.DisplayName}");
		}

		return stringBuilder.ToString();
    }

    public void TryExecute()
    {
        if (_gameObject is not null)
        {
            TryExecute(_gameObject.ActionHandlers);
        }
    }

    public abstract void TryExecute(ActionHandlers actionHandlers);
}