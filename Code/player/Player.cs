using Godot;
public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float SprintSpeed = 8.0f;
    [Export] public float JumpForce = 4.5f;
    [Export] public float MouseSensitivity = 0.002f;
    
    private Camera3D _camera;
    private Vector3 _velocity = Vector3.Zero;
    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    
    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Captured; // Lock mouse for FPS view
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            RotateY(-mouseEvent.Relative.X * MouseSensitivity);
            _camera.RotateX(-mouseEvent.Relative.Y * MouseSensitivity);
            _camera.RotationDegrees = new Vector3(Mathf.Clamp(_camera.RotationDegrees.X, -90, 90), _camera.RotationDegrees.Y, _camera.RotationDegrees.Z);
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
        Vector3 direction = Vector3.Zero;

        // Get movement input
        direction.Z = Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward");
        direction.X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

        // Normalize diagonal movement
        if (direction.Length() > 1)
            direction = direction.Normalized();

        // Convert local to global direction
        direction = Transform.Basis * direction;
        direction.Y = 0; // Prevent flying
        
        // Apply movement
        float currentSpeed = Input.IsActionPressed("sprint") ? SprintSpeed : Speed;
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
