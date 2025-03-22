using Godot;

public partial class Player : CharacterBody3D
{
    [Export] public float MoveSpeed = 5f;
    [Export] public float RotationSpeed = 10f;
    public Storage storage = new Storage(20);
    private Vector3 _velocity = Vector3.Zero;
    private float _gravity = 10f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDir = new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
        );

        if (inputDir.Length() > 1)
            inputDir = inputDir.Normalized(); // Normalize diagonal movement

        Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y);

        // Apply gravity
        if (!IsOnFloor())
            _velocity.Y -= _gravity * (float)delta; // Default gravity value (adjust as needed)
        else
            _velocity.Y = 0; // Reset gravity when on the floor

        if (direction.Length() > 0)
        {
            _velocity.X = direction.X * MoveSpeed;
            _velocity.Z = direction.Z * MoveSpeed;

            // Rotate to face movement direction
            Rotation = new Vector3(
                Rotation.X,
                Mathf.LerpAngle(Rotation.Y, Mathf.Atan2(direction.X, direction.Z), (float)(RotationSpeed * delta)),
                Rotation.Z
            );
        }
        else
        {
            _velocity.X = 0;
            _velocity.Z = 0;
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

}
