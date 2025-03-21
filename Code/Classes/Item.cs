using Godot;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; } = "Item";
    [Export] public Texture2D Icon { get; set; }
    [Export] public int MaxStackSize { get; set; } = 100; // Default stack size
    
    [Export] public BuildingData BuildingData; // Optional link to a building

    public Item(int id, string name, string iconPath)
    {
        Id = id;
        Name = name;
        Icon = ResourceLoader.Load<Texture2D>(iconPath);
    }
}
