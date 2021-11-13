namespace Amolenk.GameATron4000.Model.State;

public class GlobalState : StateManager
{
    public List<Item> Items { get; set; } = new();
    public List<Actor> Actors { get; set; } = new();
    public List<Room> Rooms { get; set; } = new();
    public Room? CurrentRoom { get; set; }
    public Room? PreviousRoom { get; set; }
    public Actor? Protagonist { get; set; }
}
