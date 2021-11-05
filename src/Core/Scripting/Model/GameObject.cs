namespace Amolenk.GameATron4000.Scripting.Model;

public abstract class GameObject
{
    public string Id { get; }

    internal ActionHandlers ActionHandlers { get; private set; }


    protected GameObject(string id, ActionHandlers actionHandlers)
    {
        Id = id;
        ActionHandlers = actionHandlers;
    }
}
