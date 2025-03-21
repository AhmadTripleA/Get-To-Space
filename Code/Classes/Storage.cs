using Godot;
using System.Collections.Generic;

public partial class Storage : Node
{
    [Export] public int Capacity = 20; // Default inventory size
    private List<ItemStack> _items = new();

    public Storage(int capacity)
    {
        Capacity = capacity;
        _items = new();
    }

    public override void _Ready()
    {
        // Initialize empty slots
        for (int i = 0; i < Capacity; i++)
        {
            _items.Add(null); // Empty slot
        }
    }


    /// <summary> Transfers items from one storage to another. </summary>
    /// <param name="from">The source storage.</param>
    /// <param name="to">The destination storage.</param>
    /// <param name="item">The item to transfer.</param>
    /// <param name="quantity">The number of items to transfer.</param>
    /// <returns>The number of items successfully transferred, if 0, then none were transfered</returns>
    public static int TransferItem(Storage from, Storage to, Item item, int quantity)
    {
        int removed = from.RemoveItem(item, quantity);
        int remaining = to.AddItem(item, removed);

        if (remaining > 0)
        {
            // If not all items were transferred, give back the extra to the original storage
            from.AddItem(item, remaining);
        }

        return removed - remaining; // Return the number of items successfully transferred
    }


    public int AddItem(Item item, int quantity)
    {
        // Try adding to an existing stack
        foreach (var stack in _items)
        {
            if (stack != null && stack.Item == item)
            {
                quantity = stack.AddToStack(quantity); // Remaining items after adding
                if (quantity == 0) return 0; // All items added
            }
        }

        // Add to an empty slot
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null)
            {
                int added = Mathf.Min(quantity, item.MaxStackSize);
                _items[i] = new ItemStack(item, added);
                quantity -= added;
                if (quantity == 0) return 0; // Fully added
            }
        }

        return quantity; // Return remaining items that couldn't fit
    }


    public int RemoveItem(Item item, int quantity)
    {
        int totalRemoved = 0;

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null && _items[i].Item == item)
            {
                int removed = _items[i].RemoveFromStack(quantity);
                totalRemoved += removed;
                quantity -= removed;

                if (_items[i].IsEmpty())
                {
                    _items[i] = null; // Remove empty stacks
                }

                if (quantity <= 0) return totalRemoved; // Stop when fully removed
            }
        }

        return totalRemoved; // Return how many were removed
    }

    public void PrintStorage()
    {
        GD.Print("Storage contents:");
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null)
                GD.Print($"{i}: {_items[i].Item.Name} x{_items[i].Quantity}");
            else
                GD.Print($"{i}: Empty");
        }
    }
}
