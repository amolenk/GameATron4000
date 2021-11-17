namespace Amolenk.GameATron4000.Model;

public record DialogueOption(string DisplayText, string Topic)
{
    public static readonly IEnumerable<DialogueOption> None =
        Enumerable.Empty<DialogueOption>();
}
