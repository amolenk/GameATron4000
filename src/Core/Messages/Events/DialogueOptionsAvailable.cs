namespace Amolenk.GameATron4000.Messages.Events;

public record DialogueOptionsAvailable(List<DialogueOption> Options) : IEvent;
