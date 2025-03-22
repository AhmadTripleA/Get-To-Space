using Godot;

public class ItemStack
{
    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    public ItemStack()
    {
        Item = null;
        Quantity = 0;
    }
    public ItemStack(Item item, int quantity)
    {
        Item = item;
        Quantity = Mathf.Min(quantity, item.MaxStackSize);
    }

    public int AddToStack(Item item, int amount)
    {
        // trying to add different items together
        if (item != Item) return amount;

        int spaceLeft = Item.MaxStackSize - Quantity;
        int added = Mathf.Min(spaceLeft, amount);
        Quantity += added;
        return amount - added; // Remaining items that couldn't fit
    }

    public int RemoveFromStack(int amount)
    {
        int removed = Mathf.Min(amount, Quantity);
        Quantity -= removed;
        return removed;
    }

    public int SpaceLeft() => Item.MaxStackSize - Quantity;

    public bool IsEmpty() => Quantity <= 0;
}
