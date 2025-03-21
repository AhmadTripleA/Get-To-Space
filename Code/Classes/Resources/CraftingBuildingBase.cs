using Godot;

[GlobalClass]
public partial class CraftingBuildingBase : BuildingBase
{
    [Export] public float CraftingSpeed = 1f; // Speed multiplier
    [Export] public Godot.Collections.Array<Recipe> Recipes = [];
}
