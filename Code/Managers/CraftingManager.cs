using Godot;
using System.Collections.Generic;

public partial class CraftingManager : Node
{
    public static CraftingManager Instance { get; private set; }
    [Export] public float CraftingSpeed = 1.0f;
    [Export] public Player player;
    private Queue<CraftingTask> craftingQueue = [];
    private Storage storage;

    public override void _Ready()
    {
        storage = player.storage;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree(); // Prevent duplicate GameManagers
        }
    }

    public void AddToCraftingQueue(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            GD.Print("Not enough materials to craft " + recipe.Name);
            return;
        }

        // Remove ingredients from player storage
        foreach (var input in recipe.Inputs)
        {
            storage.RemoveItem(input.Item, input.Amount);
        }

        // Add crafting task to queue
        craftingQueue.Enqueue(new CraftingTask(recipe, CraftingSpeed));
        GD.Print("Added " + recipe.Name + " to the crafting queue.");
    }

    public override void _Process(double delta)
    {
        if (craftingQueue.Count > 0)
        {
            CraftingTask currentTask = craftingQueue.Peek();
            currentTask.Progress += (float)delta;

            if (currentTask.Progress >= currentTask.TotalTime)
            {
                FinishCrafting(currentTask);
            }
        }
    }

    private void FinishCrafting(CraftingTask task)
    {
        craftingQueue.Dequeue();

        // Add crafted item to player storage
        foreach (var output in task.Recipe.Outputs)
        {
            int remaining = storage.AddItem(output.Item, output.Amount);

            if (remaining > 0)
            {
                GD.Print($"Not enough space for {remaining}x {output.Item.Name} in inventory!");
            }
        }

        GD.Print("Crafting complete: " + task.Recipe.Name);
    }

    private bool CanCraft(Recipe recipe)
    {
        foreach (var input in recipe.Inputs)
        {
            if (storage.GetItemCount(input.Item) < input.Amount)
                return false;
        }
        return true;
    }
}

public class CraftingTask
{
    public Recipe Recipe { get; }
    public float TotalTime { get; }
    public float Progress { get; set; }

    public CraftingTask(Recipe recipe, float speedMultiplier)
    {
        Recipe = recipe;
        TotalTime = recipe.CraftingTime / speedMultiplier; // Speed affects time
        Progress = 0;
    }
}
