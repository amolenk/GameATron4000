namespace Amolenk.GameATron4000.Model.State;

public class RoomState : StateManager
{
    public List<GameObject> Objects { get; set; }

    public RoomState()
    {
        Objects = new();
    }
}
