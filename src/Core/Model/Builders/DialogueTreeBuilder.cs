namespace Amolenk.GameATron4000.Model.Builders;

public class DialogueTreeBuilder
{
    private readonly string _id;
    private readonly Dictionary<string, Func<DialogueTree, IEnumerable<DialogueOption>>> _topics;

    internal DialogueTreeBuilder(string id)
    {
        _id = id;
        _topics = new();
    }

    public DialogueTreeBuilder WithTopic(
        string name,
        Func<DialogueTree, IEnumerable<DialogueOption>> handler)
    {
        _topics.Add(name, handler);
        return this;
    }

    internal DialogueTree Build() => new DialogueTree(_id, _topics);
}