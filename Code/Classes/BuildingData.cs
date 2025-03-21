using Godot;

[GlobalClass]
public partial class BuildingData : Resource
{
    [Export] public int AssociatedItemId;  // Store only the ID
    [Export] public PackedScene BuildingScene;
    
    [Export] public bool IsProcessingBuilding = false;
    [Export] public Godot.Collections.Array<Recipe> Recipes = [];

    public Item GetAssociatedItem()
    {
        return ItemRegistry.GetItemById(AssociatedItemId);
    }
}
