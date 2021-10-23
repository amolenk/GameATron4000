namespace Amolenk.GameATron4000.Engine.Scripting.Model;

public class WorldScript
{
    public List<RoomScript> Rooms { get; } = new();

    public RoomScript AddRoom(string id)
    {
        var room = new RoomScript();
        Rooms.Add(room);

        return room;
    }
}
