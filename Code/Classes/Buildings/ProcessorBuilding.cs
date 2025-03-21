using Godot;

[GlobalClass]
public partial class ProcessingBuilding : Building
{
    [Export] public Recipe ProcessingRecipe;
    [Export] public float ProcessingSpeed = 1f; // Speed multiplier
}
