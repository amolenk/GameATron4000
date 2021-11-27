namespace Amolenk.GameATron4000.Model.Actions;

public class GiveAction : IAction
{
    private readonly Game _game;
    private Item? _item;
    private Actor? _actor;

    public bool DisableUIWhileExecuting => true;

    public GiveAction(Game game)
    {
        _game = game;
    }

    public bool Add(GameObject gameObject)
    {
        if (_item is null)
        {
            if (gameObject is Item item &&
                _game.TryGetOwnerForItem(item, out _))
            {
                _item = item;
            }
            return false;
        }

        if (gameObject is Actor actor)
        {
            _actor = actor;
            return true;
        }

        return false;
    }

    public GameObject? GetObjectToMoveTo() => _actor;

    public string GetDisplayText(GameObject? overObject)
    {
		var stringBuilder = new StringBuilder("Give");
		
        if (_item is not null)
        {
            stringBuilder.Append($" {_item.DisplayName} to");
        }

		if (overObject is not null)
		{
			stringBuilder.Append($" {overObject.DisplayName}");
		}

		return stringBuilder.ToString();
    }

    public bool TryExecute()
    {
        if (_item is { When.HandleGive: not null} &&
            _actor is not null)
        {
            _item.When.HandleGive(_actor);
            return true;
        }
        return false;
    }
}