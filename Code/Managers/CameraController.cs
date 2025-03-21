using Godot;

public partial class CameraController : Camera3D
{
    [Export] public Node3D Target; // Assign the player in the inspector
    [Export] public Vector3 Offset = new(0, 10, -10); // Angled top-down offset
    [Export] public float Smoothness = 0.1f; // Lower = smoother follow

    public override void _Process(double delta)
    {
        if (Target == null) return;

        // Target position with offset
        Vector3 targetPos = Target.GlobalTransform.Origin + Offset;

        // Use smoothness correctly for interpolation
        GlobalTransform = new(
            GlobalTransform.Basis,
            GlobalTransform.Origin.Lerp(targetPos, Smoothness)
        );
    }
}
