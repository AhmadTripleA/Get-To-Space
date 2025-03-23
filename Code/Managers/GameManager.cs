using Godot;

public partial class GameManager : Node
{
    [Export] public BuildingManager buildingManager;
    [Export] public CraftingManager craftingManager;

    public override void _EnterTree()
    {
        StartGame();
    }

    private void StartGame()
    {
        ItemDB.LoadItems();
        RecipeDB.LoadRecipes();

        ServiceDB.Register(craftingManager);
        ServiceDB.Register(buildingManager);
    }
}
