namespace Amolenk.GameATron4000.Model.Actions;

public record Verb(string Text)
{
    public static readonly Verb Give = new Verb("Give");
    public static readonly Verb PickUp = new Verb("Pick up");
    public static readonly Verb Use = new Verb("Use");
    public static readonly Verb Open = new Verb("Open");
    public static readonly Verb LookAt = new Verb("Look at");
    public static readonly Verb Push = new Verb("Push");
    public static readonly Verb Close = new Verb("Close");
    public static readonly Verb TalkTo = new Verb("Talk to");
    public static readonly Verb Pull = new Verb("Pull");
    public static readonly Verb WalkTo = new Verb("Walk to");
}
