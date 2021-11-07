﻿namespace Amolenk.GameATron4000.Model;

public class Actor : GameObject
{
    public string TextColor { get; }

    internal Actor(ActorBuilder builder)
        : base(builder)
    {
        TextColor = builder.TextColor;
    }

    public void MoveTo(Point position)
    {
        Position = position;

        Game.EventQueue.Enqueue(new ActorMoved(this, position));
    }

    public void MoveTo(GameObject gameObject)
    {
        if (gameObject.InteractPosition != RelativePosition.None)
        {
            var dY = 0;
            if (gameObject.InteractPosition == RelativePosition.InFront)
            {
                dY = 20;
            }
            else if (gameObject.InteractPosition == RelativePosition.Above)
            {
                dY = -20;
            }

            MoveTo(gameObject.Position.Offset(0, dY));
        }
    }

    public void SayLine(string line)
    {
        Game.EventQueue.Enqueue(new SayLineActionExecuted(this, line));
    }
}
