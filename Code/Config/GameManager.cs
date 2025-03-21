using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            StartGame();
        }
        else
        {
            QueueFree(); // Prevent duplicate GameManagers
        }
    }

    private void StartGame()
    {
        ItemRegistry.LoadItems(); // Load items at start
    }
}
