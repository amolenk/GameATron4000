namespace Amolenk.GameATron4000.Model.Actions;

public interface IAction
{
    bool DisableUIWhileExecuting { get; }

    bool Add(GameObject gameObject);

    GameObject? GetObjectToMoveTo();

    string GetDisplayText(GameObject? overObject = null);

    bool TryExecute();
}
