using Godot;

public partial class CraftMenu : Control
{
    [Export] private GridContainer GridContainer; // Assign in the Inspector
    [Export] private PackedScene ItemButtonPrefab; // A reusable button template
    [Export] private Button MenuToggleButton; // Button to toggle menu

    private bool isOpen = false;

    public override void _Ready()
    {
        MenuToggleButton.Pressed += ToggleMenu;
        RecipeDB.OnRecipesLoaded += PopulateMenu;
        InputManager.CancelAction += CloseMenu;
        Hide(); // Start hidden
    }

    static void OnRecipeSelected(Recipe selectedRecipe)
    {
        GD.Print($"Selected Recipe: {selectedRecipe.Name}");
        // BuildingManager.Instance.StartPlacing(selectedRecipe.BuildingScene);
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        Visible = isOpen;
    }
    private void CloseMenu()
    {
        isOpen = false;
        Hide();
    }

    private void PopulateMenu()
    {
        GD.Print("Started Added Recipes");
        foreach (var recipe in RecipeDB.GetAllRecipes())
        {
            GD.Print($"Added new Recipe: {recipe.Name}");
            AddRecipeButton(recipe);
        }
    }

    private void AddRecipeButton(Recipe recipe)
    {
        // Instance a new button from the prefab
        TextureButton button = ItemButtonPrefab.Instantiate<TextureButton>();
        button.TextureNormal = recipe.Icon; // Set icon
        button.Pressed += () => OnRecipeSelected(recipe); // Click action

        GridContainer.AddChild(button);
    }
}
