namespace Amolenk.GameATron4000.Model.Actions;

public interface IAction
{
    bool Add(GameObject gameObject);

    GameObject? GetObjectToWalkTo();

    string GetDisplayText(GameObject? overObject = null);

    void TryExecute();
}
