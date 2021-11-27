namespace Amolenk.GameATron4000.Model;

public class ItemDependency
{
    public ItemDependency(Item item, string status)
    {
        Item = item;
        Status = status;
    }

    public Item Item { get; set; }

    public string Status { get; set; }
}
//(Item Item, string Status);
