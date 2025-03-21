using Godot;

[GlobalClass]
public partial class Recipe : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public float CraftingTime { get; set; } = 1f;

    [Export] public Godot.Collections.Array<RecipeEntry> Inputs = [];
    [Export] public Godot.Collections.Array<RecipeEntry> Outputs = [];

    public Recipe(int id, string name, string iconPath, float craftingTime)
    {
        Id = id;
        Name = name;
        Icon = ResourceLoader.Load<Texture2D>(iconPath);
        CraftingTime = craftingTime;
    }
}

