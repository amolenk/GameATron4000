namespace Amolenk.GameATron4000.Scripting.Model;

public class Collector<T> : ICollector<T>
{
    private ICollection<T> _items;

    public Collector(ICollection<T> items)
    {
        _items = items;
    }

    public void Add(T item) => _items.Add(item);
}
