using Godot;
using System;

public partial class InputManager : Node
{
    public static event Action InventoryAction;
    public static event Action PrimaryClickAction;
    public static event Action CancelAction;
    public static event Action BuildAction;

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
