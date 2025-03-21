using Godot;
using Godot.Collections;

public partial class CraftingBuilding : Building
{
    [Export] public Array<Recipe> Recipes { get; set; } = [];
    [Export] public Storage Input { get; set; } = new Storage(1);
    [Export] public Storage Output { get; set; } = new Storage(1);
}
