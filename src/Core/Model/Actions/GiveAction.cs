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
        if (!IsValidNextObject(gameObject))
        {
            return false;
        }

        if (_item is null)
        {
           _item = (Item)gameObject;
            return false;
        }

        _actor = (Actor)gameObject;
        return true;
    }

    public GameObject? GetObjectToMoveTo() => _actor;

    public string GetDisplayText(GameObject? overObject)
    {
		var stringBuilder = new StringBuilder("Give");
		
        if (_item is null)
        {
            if (IsValidNextObject(overObject))
            {
                stringBuilder.Append($" {overObject!.DisplayName}");
            }
        }
        else
        {
            stringBuilder.Append($" {_item.DisplayName} to");

            if (IsValidNextObject(overObject))
            {
                stringBuilder.Append($" {overObject!.DisplayName}");
            }
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

    private bool IsValidNextObject(GameObject? gameObject)
    {
        if (gameObject is null)
        {
            return false;
        }

        if (_item is null)
        {
            return gameObject is Item item &&
                _game.TryGetOwnerForItem(item, out Actor _);
        }

        return gameObject is Actor;
    }
}