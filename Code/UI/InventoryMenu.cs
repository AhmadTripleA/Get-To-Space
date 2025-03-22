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
                Button inventorySlot = InventorySlotPrefab.Instantiate<Button>();
                inventorySlot.Icon = stack.Item.Icon;
                inventorySlot.Text = stack.Quantity.ToString();
                if (stack.Item.BuildingScene != null)
                    inventorySlot.Pressed += () => BuildingManager.Instance.InitBuilding(stack.Item);
                GridContainer.AddChild(inventorySlot);
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
