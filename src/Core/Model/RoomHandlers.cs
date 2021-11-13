namespace Amolenk.GameATron4000.Model.Actions;

public class RoomHandlers
{
    public Action? HandleBeforeEnter { get; private set; }

    public RoomHandlers(Action? handleBeforeEnter)
    {
        HandleBeforeEnter = handleBeforeEnter;
    }
}
