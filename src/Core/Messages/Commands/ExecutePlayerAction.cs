namespace Amolenk.GameATron4000.Messages.Commands;

public record ExecutePlayerAction(IAction Action) : ICommand;
