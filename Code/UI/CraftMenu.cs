using Godot;

public partial class CraftMenu : Control
{
    [Export] private GridContainer GridContainer; // Assign in the Inspector
    [Export] private PackedScene ItemButtonPrefab; // A reusable button template

    private bool isOpen = false;

    public override void _Ready()
    {
        RecipeDB.OnRecipesLoaded += PopulateMenu;
    }

    void OnRecipeSelected(Recipe selectedRecipe)
    {
        GD.Print($"Selected Recipe: {selectedRecipe.Name}");

        CraftingManager craftingManager = ServiceDB.Get<CraftingManager>();
        craftingManager?.AddToCraftingQueue(selectedRecipe);
    }

    private void PopulateMenu()
    {
        foreach (var recipe in RecipeDB.GetAll())
        {
            if (recipe.IsUnlocked)
            {
                // Instance a new button from the prefab
                ItemSlotBtn btn = ItemButtonPrefab.Instantiate<ItemSlotBtn>();
                btn.Construct(recipe.Outputs[0].Item.Icon, recipe.Name, "");

                btn.Pressed += () => OnRecipeSelected(recipe); // Click action

                GridContainer.AddChild(btn);

                GD.Print($"Added new Recipe: {recipe.Name}");
            }
        }
    }
}
