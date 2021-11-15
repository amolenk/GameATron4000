namespace Amolenk.GameATron4000.Model.Actions;

public interface IAction
{
    bool Add(IGameObject gameObject);

    IGameObject? GetObjectToWalkTo();

    string GetDisplayText(IGameObject? overObject = null);

    void TryExecute();
}
