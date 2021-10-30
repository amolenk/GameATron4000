namespace Amolenk.GameATron4000.Scripting.Model;

public abstract class GameObject
{
    public string Id { get; }

    protected GameObject(string id)
    {
        Id = id;
    }
}
