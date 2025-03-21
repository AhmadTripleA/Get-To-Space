using Godot;

[GlobalClass]
public partial class Building : Node3D
{
    [Export] public BuildingBase Base { get; private set; } // Assigned when placed
}
