namespace Amolenk.GameATron4000.Model.Builders;

public class ActorBuilder : GameObjectBuilder<Actor, ActorBuilder>
{
    public string TextColor { get; private set; }

    internal ActorBuilder() : base()
    {
        Status = WellKnownStatus.FaceCamera;
        TextColor = "white";
    }

    public ActorBuilder WithTextColor(string textColor)
    {
        TextColor = textColor;
        return this;
    }
}
