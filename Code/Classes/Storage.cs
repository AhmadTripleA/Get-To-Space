using Godot;
using System.Collections.Generic;

public partial class Storage : Node
{
    [Export] public int Capacity = 20; // Default inventory size
    private List<ItemStack> _items = [];

    public Storage(int capacity)
    {
        Capacity = capacity;
        _items = [];
    }

    public override void _Ready()
    {
        // Initialize empty slots
        for (int i = 0; i < Capacity; i++)
        {
            _items.Add(null); // Empty slot
        }
    }

    /// <summary>
    /// Transfers items from one storage to another.
    /// </summary>
    public static int TransferItem(Storage from, Storage to, Item item, int quantity)
    {
        int removed = from.RemoveItem(item, quantity);
        int remaining = to.AddItem(item, removed);

        if (remaining > 0)
        {
            // If not all items were transferred, return the extra to the original storage
            from.AddItem(item, remaining);
        }

        return removed - remaining; // Number of successfully transferred items
    }

    /// <summary>
    /// Adds an item to the storage, respecting stack sizes and capacity.
    /// </summary>
    /// <returns>Returns the number of items that could NOT be added.</returns>
    public int AddItem(Item item, int quantity)
    {
        // Try adding to an existing stack
        foreach (var stack in _items)
        {
            if (stack != null)
            {
                quantity = stack.AddToStack(item, quantity); // Remaining items after adding
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

    /// <summary>
    /// Removes an item from storage.
    /// </summary>
    /// <returns>Returns the number of items successfully removed.</returns>
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

    /// <summary>
    /// Gets the total count of a specific item in storage.
    /// </summary>
    public int GetItemCount(Item item)
    {
        int total = 0;
        foreach (var stack in _items)
        {
            if (stack != null && stack.Item == item)
            {
                total += stack.Quantity;
            }
        }
        return total;
    }

    /// <summary>
    /// Checks if the storage has enough space to store a given item.
    /// </summary>
    public bool CanStoreItem(Item item, int quantity)
    {
        int remaining = quantity;

        // Check existing stacks for space
        foreach (var stack in _items)
        {
            if (stack != null && stack.Item == item)
            {
                remaining = stack.SpaceLeft(remaining);
                if (remaining == 0) return true; // Can fully store
            }
        }

        // Check for empty slots
        foreach (var slot in _items)
        {
            if (slot == null)
            {
                return true; // Empty slot available
            }
        }

        return false; // No space left
    }

    /// <summary>
    /// Debug method to print storage contents.
    /// </summary>
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
