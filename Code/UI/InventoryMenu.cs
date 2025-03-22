using System;
using Godot;

public partial class InventoryMenu : Control
{
    [Export] public Player player;
    [Export] private GridContainer GridContainer;
    [Export] private PackedScene InventorySlotPrefab;

    private Storage storage;
    private bool isOpen = false;

    public override void _Ready()
    {
        storage = player.storage;
        storage.Connect(Storage.SignalName.InventoryChanged, new Callable(this, nameof(PopulateUI)));
        PopulateUI();
    }

    private void PopulateUI()
    {
        GD.Print("Updating Inventory UI...");

        foreach (Node child in GridContainer.GetChildren()) child.QueueFree();

        foreach (var stack in storage.GetAllStacks())
        {
            if (stack != null)
            {
                Button slot = InventorySlotPrefab.Instantiate<Button>();
                slot.Icon = stack.Item.Icon;
                slot.Text = stack.Quantity.ToString();
                GridContainer.AddChild(slot);
            }
        }

        storage.PrintStorage();
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        Visible = isOpen;
    }
}
