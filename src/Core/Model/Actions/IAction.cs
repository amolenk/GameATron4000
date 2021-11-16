namespace Amolenk.GameATron4000.Model.Actions;

public interface IAction
{
    bool Add(IGameObject gameObject);

    // TODO ...ToMoveTo  
    IGameObject? GetObjectToWalkTo();

    string GetDisplayText(IGameObject? overObject = null);

    bool TryExecute();
}
