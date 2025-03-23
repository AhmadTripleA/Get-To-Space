using Godot;

public partial class BuildMenu : Control
{
    [Export] private GridContainer GridContainer; // Assign in the Inspector
    [Export] private PackedScene ItemButtonPrefab; // A reusable button template

    private bool isOpen = false;

    public override void _Ready()
    {
        ItemDB.OnItemsLoaded += PopulateMenu;
    }

    static void OnItemSelected(Item selectedItem)
    {
        BuildingManager buildingManager = ServiceDB.Get<BuildingManager>();
        buildingManager?.InitBuilding(selectedItem);
    }

    private void PopulateMenu()
    {
        GD.Print("Started Added Buildings");
        foreach (var item in ItemDB.GetAllItems())
        {
            if (item.BuildingScene != null) // Filter only buildings
            {
                GD.Print($"Added new Item: {item.Name}");
                AddItemButton(item);
            }
        }
    }

    private void AddItemButton(Item item)
    {
        // Instance a new button from the prefab
        TextureButton button = ItemButtonPrefab.Instantiate<TextureButton>();
        button.TextureNormal = item.Icon; // Set icon
        button.Pressed += () => OnItemSelected(item); // Click action

        GridContainer.AddChild(button);
    }
}
