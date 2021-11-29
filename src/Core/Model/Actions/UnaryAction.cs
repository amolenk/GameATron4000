namespace Amolenk.GameATron4000.Model.Actions;

public abstract class UnaryAction : IAction
{
    private readonly Game _game;
    private readonly Verb _verb;
    private readonly bool _allowInventoryItems;
    private GameObject? _gameObject;

    public bool DisableUIWhileExecuting => true;

    protected UnaryAction(
        Game game,
        Verb verb,
        bool allowInventoryItems = false)
    {
        _game = game;
        _verb = verb;
        _allowInventoryItems = allowInventoryItems;
    }

    public bool Add(GameObject gameObject)
    {
        if (IsAllowed(gameObject))
        {
            _gameObject = gameObject;
            return true;
        }
        return false;
    }

    public GameObject? GetObjectToMoveTo()
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
		
		if (gameObject is not null && IsAllowed(gameObject))
		{
			stringBuilder.Append($" {gameObject.DisplayName}");
		}

		return stringBuilder.ToString();
    }

    public bool TryExecute()
    {
        if (_gameObject is not null)
        {
            var handler = GetHandler(_gameObject.ActionHandlers);
            if (handler is not null)
            {
                handler();
                return true;
            }
        }
        return false;
    }

    public abstract Action? GetHandler(ActionHandlers actionHandlers);

    protected bool IsAllowed(GameObject gameObject) =>
        gameObject is Actor ||
        _allowInventoryItems ||
        (gameObject is Item item &&
            !_game.TryGetOwnerForItem(item, out Actor _));
}