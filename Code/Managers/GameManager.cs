using Godot;

public partial class GameManager : Node
{
    [Export] public BuildingManager buildingManager;
    [Export] public CraftingManager craftingManager;

    public override void _Ready()
    {
        StartGame();
    }

    private void StartGame()
    {
        ItemDB.LoadAll();
        RecipeDB.LoadAll();
        ResearchDB.LoadAll();

        ServiceDB.Register(craftingManager);
        ServiceDB.Register(buildingManager);
    }
}
