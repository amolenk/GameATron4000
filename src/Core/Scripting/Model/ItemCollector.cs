namespace Amolenk.GameATron4000.Scripting.Model;

public class ItemCollector<T>
{
    private ICollection<T> _items;

    public ItemCollector(ICollection<T> items)
    {
        _items = items;
    }

    public void Add(T item) => _items.Add(item);
}
