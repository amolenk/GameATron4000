namespace Amolenk.GameATron4000.Scripting.Model;

public class ActorBuilder : GameObjectBuilder<Actor, ActorBuilder>
{
    public string TextColor { get; private set; }

    internal ActorBuilder(
        string id,
        ICollector<IEvent> events,
        ICollector<GameObject> gameObjects)
        : base(id, events, gameObjects)
    {
        State = WellKnownState.FaceCamera;
        TextColor = "White";
    }

    public ActorBuilder WithTextColor(string textColor)
    {
        TextColor = textColor;
        return this;
    }

    protected override Actor Build() => new Actor(this);
}