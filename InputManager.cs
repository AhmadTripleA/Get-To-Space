using Godot;
using System;

public partial class InputManager : Node
{
    // Singleton pattern to access InputManager globally
    private static InputManager _instance;
    public static InputManager Instance => _instance;

    // Input events are broadcast using signals
    public static event Action JumpPressed;
    public static event Action PrimaryClickAction;
    public static event Action CancelAction;
    public static event Action MoveUpPressed;

    public override void _Ready()
    {
        // Assigning the singleton instance
        if (_instance == null)
        {
            _instance = this;
            // Optionally, set it to be an AutoLoad in the project settings to make it persistent across scenes
            // Add to scene tree to allow input to be captured
            // GetTree().Root.AddChild(this);
        }
    }

    public override void _Process(double delta)
    {
        // Example of checking for player input in the _Process method
        if (Input.IsActionJustPressed("ui_up")) // Default is up arrow key or W key
        {
            MoveUpPressed?.Invoke();
        }

        if (Input.IsActionJustPressed("jump")) // You can map this in the Input Map (default space key)
        {
            JumpPressed?.Invoke();
        }

        if (Input.IsActionJustPressed("ui_cancel")) // You can map this in the Input Map (default space key)
        {
            CancelAction?.Invoke();
        }

        if (Input.IsActionJustPressed("ui_accept")) // You can map this in the Input Map (default space key)
        {
            PrimaryClickAction?.Invoke();
        }
    }
}
