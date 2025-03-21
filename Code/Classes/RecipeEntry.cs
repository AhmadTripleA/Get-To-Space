using Godot;

[GlobalClass]
public partial class RecipeEntry : Resource
{
    [Export] public Item Item;
    [Export] public float Quantity;
}