namespace Amolenk.GameATron4000.Engine.Scripting.Model;

public class Room
{
    public string Id { get; }
    public Walkbox Walkbox { get; }

    public Room(
        string id,
        Walkbox walkbox)
    {
        Id = id;
        Walkbox = walkbox;
    }

    public void Enter()
    {
        //IPublisher RoomEntereted
    }
}
