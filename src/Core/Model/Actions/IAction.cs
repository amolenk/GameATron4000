namespace Amolenk.GameATron4000.Model.Actions;

public interface IAction
{
    bool Add(GameObject gameObject);

    // TODO ...ToMoveTo  
    GameObject? GetObjectToWalkTo();

    string GetDisplayText(GameObject? overObject = null);

    bool TryExecute();
}
