namespace Amolenk.GameATron4000.Scripting.Model;

public class ActionHandlers
{
    public Action? HandleLookAt { get; private set; }

    public void LookAt(Action handler) => HandleLookAt = handler;
}
