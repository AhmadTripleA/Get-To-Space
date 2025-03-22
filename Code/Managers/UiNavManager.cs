using Godot;

public partial class UiNavManager : Control
{
    [Export] public Control InventoryControl;
    [Export] public Control BuildControl;
    private bool isInventoryOpen = false;
    private bool isBuildOpen = false;

    public override void _Ready()
    {
        InputManager.BuildAction += () => ToggleUI(BuildControl);
        InputManager.InventoryAction += () => ToggleUI(InventoryControl);
        InputManager.CancelAction += () =>
        {
            CloseUI(InventoryControl);
            CloseUI(BuildControl);
        };
        BuildControl.Hide();
        InventoryControl.Hide();
    }

    public static void ToggleUI(Control control)
    {
        control.Visible = !control.Visible;
    }
    public static void CloseUI(Control control)
    {
        control.Hide();
    }
}


