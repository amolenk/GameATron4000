namespace Amolenk.GameATron4000.Scripting.Model;

public class RoomActionHandlers
{
    public Action? HandleBeforeEnter { get; private set; }

    public void BeforeEnter(Action handler) => HandleBeforeEnter = handler;
}
