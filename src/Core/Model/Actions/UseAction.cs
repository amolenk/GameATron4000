namespace Amolenk.GameATron4000.Model.Actions;

public class UseAction : IAction
{
    private readonly Game _game;
    private Item? _item;
    private GameObject? _with;

    public UseAction(Game game)
    {
        _game = game;
    }

    public bool Add(GameObject gameObject)
    {
        // If an item is already set, only accept the object if the set item can
        // be used in combination with another item.
        if (_item is { CanBeUsedWithOtherObject: true } && _item != gameObject)
        {
            _with = gameObject;
            return true;
        }
        // If an item is not already set, set the object if it's an item.
        else if (gameObject is Item item)
        {
            _item = item;
            return !item.CanBeUsedWithOtherObject;
        }

        return false;
    }

    public GameObject? GetObjectToWalkTo()
    {
        // If a 'with' object is set, walk to it if it isn't owned by any actor.
        if (_with is Actor ||
            (_with is Item item && !_game.TryGetOwnerForItem(item, out Actor _)))
        {
            return _with;
        }

        // If an item is set, also check if it isn't in some actor's inventory.
        if (_item is Item && !_game.TryGetOwnerForItem(_item, out Actor _))
        {
            return _item;
        }

        return null;
    }

    public string GetDisplayText(GameObject? overObject)
    {
		var stringBuilder = new StringBuilder("Use");
		
        // Ignore the current room object that the mouse is over if it's the
        // same as the set item.
        if (overObject == _item)
        {
            overObject = null;
        }

        if (_item is not null)
        {
            stringBuilder.Append($" {_item.DisplayName}");

            if (_item.CanBeUsedWithOtherObject)
            {
                stringBuilder.Append($" with");
            }
        }

        if (overObject is not null)
        {
            stringBuilder.Append($" {overObject.DisplayName}");
        }

		return stringBuilder.ToString();
    }

    public bool TryExecute()
    {
        if (_item is { ActionHandlers.HandleUse: not null} &&
            (!_item.CanBeUsedWithOtherObject || _with is not null))
        {
            _item.ActionHandlers.HandleUse(_with);
            return true;
        }
        return false;
    }
    
    private bool IsUseWith() =>
        _item is Item item &&
        _game.TryGetOwnerForItem(item, out Actor _) &&
        _item.CanBeUsedWithOtherObject;
}