using Godot;

public partial class ItemStack
{
    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    public ItemStack(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    public int AddToStack(int amount)
    {
        int spaceLeft = Item.MaxStackSize - Quantity;
        int addedAmount = Mathf.Min(amount, spaceLeft);
        Quantity += addedAmount;
        return amount - addedAmount; // Return remaining amount that couldn't fit
    }

    public int RemoveFromStack(int amount)
    {
        int removedAmount = Mathf.Min(amount, Quantity);
        Quantity -= removedAmount;
        return removedAmount;
    }

    public bool IsEmpty() => Quantity <= 0;
}
