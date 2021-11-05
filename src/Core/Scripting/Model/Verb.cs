namespace Amolenk.GameATron4000.Scripting.Model;

public record Verb(string Text)
{
    public static readonly Verb LookAt = new Verb("Look at");
    public static readonly Verb WalkTo = new Verb("Walk to");
}
