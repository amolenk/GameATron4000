namespace Amolenk.GameATron4000.Model;

public class ItemDependency
{
    public ItemDependency(Func<Item> getItem, string status)
    {
        GetItem = getItem;
        Status = status;
    }

    public Func<Item> GetItem { get; set; }

    public string Status { get; set; }
}
