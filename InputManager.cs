using Godot;
using System;

public partial class InputManager : Node
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;
    public static event Action PrimaryClickAction;
    public static event Action CancelAction;

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
