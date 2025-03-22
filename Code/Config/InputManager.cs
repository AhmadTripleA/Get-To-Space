using Godot;
using System;

public partial class InputManager : Node
{
    private static InputManager instance;
    public static InputManager Instance => instance;
    public static event Action PrimaryClickAction;
    public static event Action CancelAction;

    public override void _Ready()
    {
        // Assigning the singleton instance
        if (instance == null)
        {
            instance = this;
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
