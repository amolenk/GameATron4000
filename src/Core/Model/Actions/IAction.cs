namespace Amolenk.GameATron4000.Model.Actions;

public interface IAction
{
    bool Add(GameObject gameObject);

    GameObject? GetInteractObject();

    string GetDisplayText(GameObject? hoverObject);

    void Execute();
}
