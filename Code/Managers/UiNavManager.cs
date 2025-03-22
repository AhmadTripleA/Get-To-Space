using System.Collections.Generic;
using Godot;

public partial class UiNavManager : Control
{
    [Export] public Control InventoryControl;
    [Export] public Control BuildControl;

    private static List<Control> allControls = [];
    private bool isInventoryOpen = false;
    private bool isBuildOpen = false;

    public override void _Ready()
    {
        allControls.Add(BuildControl);
        allControls.Add(InventoryControl);

        InputManager.BuildAction += () => ToggleUI(BuildControl);
        InputManager.InventoryAction += () => ToggleUI(InventoryControl);
        InputManager.CancelAction += CloseAll;

        HideAll();
    }

    public static void ToggleUI(Control control)
    {
        control.Visible = !control.Visible;
    }
    public static void CloseUI(Control control)
    {
        control.Hide();
    }
    public static void CloseAll()
    {
        foreach (Control control in allControls)
        {
            control.Visible = false;
        }
    }

    public static void HideAll()
    {
        foreach (Control control in allControls)
        {
            control.Hide();
        }
    }
}


