namespace Amolenk.GameATron4000.Model;

public class DialogueTree
{
    public const string StartTopicName = "_startTopic";

    private readonly Dictionary<string, Func<DialogueTree, IEnumerable<DialogueOption>>> _topics;

    public string Id { get; private set; }

    public DialogueTree(
        string id,
        Dictionary<string, Func<DialogueTree, IEnumerable<DialogueOption>>> topics)
    {
        Id = id;
        _topics = topics;
    }

    public IEnumerable<DialogueOption> Continue(string name)
    {
        if (_topics.TryGetValue(
            name,
            out Func<DialogueTree, IEnumerable<DialogueOption>> topic))
        {
            return topic
                .Invoke(this)
                .Where(option => option.Condition is null || option.Condition())
                .ToList();
        }

        throw new InvalidOperationException(
            $"Dialogue tree '{Id}' does not contain topic '{name}'.");
    }

    public IEnumerable<DialogueOption> End() =>
        Enumerable.Empty<DialogueOption>();

    internal IEnumerable<DialogueOption> Start() => Continue(StartTopicName);
}
