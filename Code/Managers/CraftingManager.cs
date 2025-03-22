using Godot;

public partial class CraftingManager : Node3D
{
    public static CraftingManager Instance { get; private set; }
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            InputManager.CancelAction += CleanUp;
            // InputManager.PrimaryClickAction += OnConfirmBuild;
        }
        else
        {
            QueueFree(); // Prevent duplicate GameManagers
        }
    }

    public override void _Process(double delta)
    {

    }

    private void CleanUp()
    {

    }

}
