using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class Crafter : Node
{
    [Export] public float CraftingSpeed = 1f;
    [Export] public Recipe Recipe;
    public List<Storage> Inputs = [];
    public List<Storage> Outputs = [];

    private bool isCrafting = false;
    private bool isActive = true;
    private Timer craftingTimer;

    public override void _Ready()
    {
        // Initialize storage lists
        for (int i = 0; i < Recipe.Inputs.Length; i++)
        {
            Inputs.Add(new Storage(1));
        }

        for (int i = 0; i < Recipe.Outputs.Length; i++)
        {
            Outputs.Add(new Storage(1));
        }

        // Add and configure crafting timer
        craftingTimer = new Timer();
        AddChild(craftingTimer);
        craftingTimer.OneShot = false; // Keep running indefinitely
        craftingTimer.Connect("timeout", Callable.From(OnCraftingFinished));
    }

    public override void _Process(double delta)
    {
        if (isCrafting || !isActive) return;

        if (CanStartCrafting())
        {
            StartCrafting();
        }
    }

    private bool CanStartCrafting()
    {
        // Ensure all required inputs exist
        foreach (var entry in Recipe.Inputs)
        {
            int totalAvailable = Inputs.Sum(storage => storage.GetItemCount(entry.Item));
            if (totalAvailable < entry.Quantity)
                return false; // Not enough input
        }

        // Ensure outputs can be stored
        foreach (var entry in Recipe.Outputs)
        {
            bool canStore = Outputs.Any(storage => storage.CanStoreItem(entry.Item, entry.Quantity));
            if (!canStore)
                return false; // Not enough space for output
        }

        return true;
    }

    private void StartCrafting()
    {
        isCrafting = true;

        // Remove inputs
        foreach (var entry in Recipe.Inputs)
        {
            int amountNeeded = entry.Quantity;
            foreach (var storage in Inputs)
            {
                amountNeeded -= storage.RemoveItem(entry.Item, amountNeeded);
                if (amountNeeded <= 0) break;
            }
        }

        // Start crafting timer
        float craftingTime = Recipe.CraftingTime / CraftingSpeed;
        craftingTimer.Start(craftingTime);
    }

    private void OnCraftingFinished()
    {
        // Add outputs
        foreach (var entry in Recipe.Outputs)
        {
            foreach (var storage in Outputs)
            {
                int remaining = storage.AddItem(entry.Item, entry.Quantity);
                if (remaining == 0) break; // Fully stored
            }
        }

        isCrafting = false;

        if (CanStartCrafting())
        {
            StartCrafting();
        }
    }
}
