using Godot;

[GlobalClass]
public partial class Building : Node3D
{
    [Export] public BuildingData Data { get; private set; } // Assigned when placed

    public void Init(BuildingData data)
    {
        Data = data;
        GD.Print("Building initialized: " + Data.GetAssociatedItem().Name);
    }
}
