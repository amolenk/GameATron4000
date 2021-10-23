using Amolenk.GameATron4000.Engine.Graphics;

namespace Amolenk.GameATron4000.Engine;

public class ActorTemp
{
    public string Id;
    public string Name;
    public int PositionX;
    public int PositionY;

    public ActorTemp()
    {
    }
}

public class Actor
{
    private Actor()
    {

    }

    public static async Task<Actor> CreateAsync(
        ActorTemp data,
        IGraphics graphics)
    {
        var sprite = await graphics.AddSpriteAsync(
            data.PositionX,
            data.PositionY,
            $"actors/{data.Id}/front",
            options =>
            {
                options.SetOrigin(0.5, 1);
            });

        return new Actor();
    }
}