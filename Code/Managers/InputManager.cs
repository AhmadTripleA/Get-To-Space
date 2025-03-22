using Godot;
using System;

public partial class InputManager : Node
{
    private static InputManager instance;
    public static InputManager Instance => instance;
    public static event Action InventoryAction;
    public static event Action PrimaryClickAction;
    public static event Action CancelAction;
    public static event Action BuildAction;

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
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            CancelAction?.Invoke();
        }
        if (Input.IsActionJustPressed("ui_accept"))
        {
            PrimaryClickAction?.Invoke();
        }
        if (Input.IsActionJustPressed("inventory"))
        {
            InventoryAction?.Invoke();
        }
        if (Input.IsActionJustPressed("build"))
        {
            BuildAction?.Invoke();
        }
    }
}
