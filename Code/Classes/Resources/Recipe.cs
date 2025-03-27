using Godot;

[GlobalClass]
public partial class Recipe : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public float CraftingTime { get; set; }
    [Export] public bool IsUnlocked { get; set; } = true; // true for now
    [Export] public RecipeEntry[] Inputs = [];
    [Export] public RecipeEntry[] Outputs = [];
}

public partial class RecipeEntry : Resource
{
    [Export] public Item Item;
    [Export] public int Amount;
}