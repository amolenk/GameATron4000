namespace Amolenk.GameATron4000.Model.Actions;

public class RoomHandlers
{
    public Action? HandleBeforeEnter { get; private set; }

    public Action? HandleAfterEnter { get; private set; }

    public RoomHandlers()
    {
    }

    public void BeforeEnter(Action beforeEnter)
    {
        HandleBeforeEnter = beforeEnter;
    }

    public void AfterEnter(Action afterEnter)
    {
        HandleAfterEnter = afterEnter;
    }
}
