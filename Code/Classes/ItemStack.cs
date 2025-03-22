using Godot;

public class ItemStack
{
    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    public ItemStack(Item item, int quantity)
    {
        Item = item;
        Quantity = Mathf.Min(quantity, item.MaxStackSize);
    }

    public int AddToStack(int amount)
    {
        int spaceLeft = Item.MaxStackSize - Quantity;
        int added = Mathf.Min(spaceLeft, amount);
        Quantity += added;
        return amount - added; // Return remaining items that couldn't fit
    }

    public int RemoveFromStack(int amount)
    {
        int removed = Mathf.Min(amount, Quantity);
        Quantity -= removed;
        return removed;
    }

    public int SpaceLeft(int amount)
    {
        int spaceLeft = Item.MaxStackSize - Quantity;
        return Mathf.Max(0, amount - spaceLeft); // Returns the excess that won't fit
    }

    public bool IsEmpty() => Quantity <= 0;
}
