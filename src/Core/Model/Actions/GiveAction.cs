namespace Amolenk.GameATron4000.Model.Actions;

public class GiveAction : IAction
{
    private GameObject? _inventoryItem;
    private Actor? _actor;

    public GiveAction()
    {
    }

    public bool Add(GameObject gameObject)
    {
        if (_inventoryItem is null)
        {
            if (!(gameObject is Actor))
            {
                _inventoryItem = gameObject;
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

    public GameObject? GetInteractObject() => _actor;

    public string GetDisplayText(GameObject? hoverObject)
    {
		var stringBuilder = new StringBuilder("Give");
		
        if (_inventoryItem is not null)
        {
            stringBuilder.Append($" {_inventoryItem.DisplayName} to");
        }

		if (hoverObject is not null)
		{
			stringBuilder.Append($" {hoverObject.DisplayName}");
		}

		return stringBuilder.ToString();
    }

    public void Execute() =>
        _inventoryItem!.Handlers.HandleGive?.Invoke(_actor!);
}