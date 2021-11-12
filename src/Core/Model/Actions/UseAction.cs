namespace Amolenk.GameATron4000.Model.Actions;

public class UseAction : IAction
{
    private GameObject? _gameObject1;
    private GameObject? _gameObject2;

    public UseAction()
    {
    }

    public bool Add(GameObject gameObject)
    {
        if (_gameObject1 is not null && IsUseWith())
        {
            if (!(gameObject is Actor) &&
                gameObject != _gameObject1)
            {
                _gameObject2 = gameObject;
                return true;
            }
        }
        else
        {
            if (!(gameObject is Actor))
            {
                _gameObject1 = gameObject;
                return !IsUseWith();
            }
        }

        return false;
    }

    public GameObject? GetInteractObject()
    {
        if (_gameObject1!.Owner is null)
        {
            return _gameObject1;
        }
        if (_gameObject2!.Owner is null)
        {
            return _gameObject2;
        }
        return null;
    }

    public string GetDisplayText(GameObject? hoverObject)
    {
		var stringBuilder = new StringBuilder("Use");
		
        // Ignore the current room object that the mouse is over if it's the same as
        // the first subject (if any).
        if (hoverObject == _gameObject1)
        {
            hoverObject = null;
        }

        if (_gameObject1 is not null)
        {
            stringBuilder.Append($" {_gameObject1.DisplayName}");

            if (IsUseWith())
            {
                stringBuilder.Append($" with");
            }
        }

        if (hoverObject is not null)
        {
            stringBuilder.Append($" {hoverObject.DisplayName}");
        }

		return stringBuilder.ToString();
    }

    public void Execute() =>
        _gameObject1!.Handlers.HandleUse?.Invoke(_gameObject2);
    
    private bool IsUseWith() =>
        _gameObject1 is GameObject { Owner: not null } &&
            _gameObject1.UseWith;
}