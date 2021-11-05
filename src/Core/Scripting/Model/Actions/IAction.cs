namespace Amolenk.GameATron4000.Scripting.Model.Actions;

public interface IAction
{
    Verb Verb { get; }
    GameObject? Subject { get; }

    bool Add(GameObject gameObject);

    string GetDisplayText(GameObject? gameObject);

    void Execute(ActionHandlers handlers);
}
