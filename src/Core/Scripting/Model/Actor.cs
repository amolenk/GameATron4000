namespace Amolenk.GameATron4000.Scripting.Model;

public class Actor : GameObject
{
    public string TextColor { get; }

    internal Actor(ActorBuilder builder)
        : base(builder)
    {
        TextColor = builder.TextColor;
    }

    public void SayLine(string line)
    {
        EventQueue.Enqueue(new SayLineActionExecuted(this, line));
    }
}
