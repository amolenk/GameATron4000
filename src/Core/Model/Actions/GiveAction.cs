namespace Amolenk.GameATron4000.Model.Actions;

public class GiveAction : IAction
{
    private readonly Game _game;
    private Item? _item;
    private Actor? _actor;

    public GiveAction(Game game)
    {
        _game = game;
    }

    public bool Add(IGameObject gameObject)
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

    public IGameObject? GetObjectToWalkTo() => _actor;

    public string GetDisplayText(IGameObject? overObject)
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
        if (_item is { ActionHandlers.HandleGive: not null} &&
            _actor is not null)
        {
            _item.ActionHandlers.HandleGive(_actor);
            return true;
        }
        return false;
    }
}