using Godot;
public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float SprintMultiplier = 2.0f;
    [Export] public float JumpForce = 4.5f;
    [Export] public float MouseSensitivity = 0.002f;
    [Export] public Camera3D Camera;

    private Vector3 _velocity = Vector3.Zero;
    private float _gravity = 10.0f;
    
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured; // Lock mouse for FPS view
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            RotateY(-mouseEvent.Relative.X * MouseSensitivity);
            Camera.RotateX(-mouseEvent.Relative.Y * MouseSensitivity);
            Camera.RotationDegrees = new Vector3(Mathf.Clamp(Camera.RotationDegrees.X, -90, 90), Camera.RotationDegrees.Y, Camera.RotationDegrees.Z);
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured 
                ? Input.MouseModeEnum.Visible 
                : Input.MouseModeEnum.Captured;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Get movement input
        float forwardInput = Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward");
        float rightInput = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

        // Get camera forward and right directions (ignoring Y so player doesn't tilt)
        Vector3 cameraForward = -Camera.GlobalTransform.Basis.Z; // Forward direction
        Vector3 cameraRight = Camera.GlobalTransform.Basis.X;    // Right direction

        cameraForward.Y = 0; // Prevent vertical movement
        cameraRight.Y = 0;

        cameraForward = cameraForward.Normalized();
        cameraRight = cameraRight.Normalized();

        // Compute movement direction relative to camera
        Vector3 direction = (cameraForward * forwardInput) + (cameraRight * rightInput);

        // Normalize diagonal movement
        if (direction.Length() > 1)
            direction = direction.Normalized();

        // Apply movement
        float currentSpeed = Input.IsActionPressed("sprint") ? Speed * SprintMultiplier : Speed;
        _velocity.X = direction.X * currentSpeed;
        _velocity.Z = direction.Z * currentSpeed;

        // Apply gravity
        if (!IsOnFloor())
            _velocity.Y -= _gravity * (float)delta;

        // Jumping
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            _velocity.Y = JumpForce;

        Velocity = _velocity;
        MoveAndSlide();
    }
}
