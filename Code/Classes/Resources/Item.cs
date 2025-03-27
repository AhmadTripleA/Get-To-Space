using Godot;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public int MaxStackSize { get; set; }
    [Export] public bool IsUnlocked { get; set; } = true; // true for now
    [Export] public PackedScene BuildingScene;
}
