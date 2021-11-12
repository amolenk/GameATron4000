namespace Amolenk.GameATron4000.Model;

public class GameObjectState : StateManager
{
    public string Frame { get; set; }
    public Actor? Owner { get; set; }
    public Point Position { get; set; }
    public Room? Room { get; set; }

    public GameObjectState()
    {
        Frame = "default";
        Owner = null;
        Position = new(0, 0);
        Room = null;
    }
}
