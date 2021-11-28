namespace Amolenk.GameATron4000.Model;

public record ItemDependency(Func<Item> GetItem, string Status);
