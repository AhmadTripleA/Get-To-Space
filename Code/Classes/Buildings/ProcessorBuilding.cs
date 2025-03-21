using Godot;

[GlobalClass]
public partial class ProcessingBuilding : BuildingBase
{
    [Export] public float ProcessingSpeed = 1f; // Speed multiplier
    [Export] public Godot.Collections.Array<Recipe> Recipes = [];
}
