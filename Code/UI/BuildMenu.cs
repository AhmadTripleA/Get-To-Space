using Godot;

public partial class BuildMenu : Control
{
    [Export] private GridContainer GridContainer; // Assign in the Inspector
    [Export] private PackedScene ItemButtonPrefab; // A reusable button template
    [Export] private Button MenuToggleButton; // Button to toggle menu

    private bool isOpen = false;

    public override void _Ready()
    {
        MenuToggleButton.Pressed += ToggleMenu;
        ItemDB.OnItemsLoaded += PopulateMenu;
        InputManager.CancelAction += CloseMenu;
        Hide(); // Start hidden
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        Visible = isOpen;
    }
    private void CloseMenu()
    {
        isOpen = false;
        Visible = false;
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

    private void OnItemSelected(Item selectedItem)
    {
        GD.Print($"Selected: {selectedItem.Name}");
        BuildingManager.Instance.StartPlacing(selectedItem.BuildingScene);
    }
}
