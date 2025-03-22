using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class Crafter : Node
{
    [Export] public float craftingSpeed = 1f;
    [Export] public bool loopCrafting = true;
    [Export] public Recipe selectedRecipe;
    private List<Storage> inputs = [];
    private List<Storage> outputs = [];
    private bool isCrafting = false; // is currently crafting something
    private bool isActive = true; // ????
    private Timer craftingTimer;

    public override void _Ready()
    {
        // Initialize storage lists
        for (int i = 0; i < selectedRecipe.Inputs.Length; i++)
        {
            inputs.Add(new Storage(1));
        }

        for (int i = 0; i < selectedRecipe.Outputs.Length; i++)
        {
            outputs.Add(new Storage(1));
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
        foreach (var entry in selectedRecipe.Inputs)
        {
            int totalAvailable = inputs.Sum(storage => storage.GetItemCount(entry.Item));
            if (totalAvailable < entry.Amount)
                return false; // Not enough input
        }

        // Ensure outputs can be stored
        foreach (var entry in selectedRecipe.Outputs)
        {
            bool canStore = outputs.Any(storage => storage.CanStoreItem(entry.Item, entry.Amount));
            if (!canStore)
                return false; // Not enough space for output
        }

        return true;
    }

    private void StartCrafting()
    {
        isCrafting = true;

        // Remove inputs
        foreach (var entry in selectedRecipe.Inputs)
        {
            int amountNeeded = entry.Amount;
            foreach (var storage in inputs)
            {
                amountNeeded -= storage.RemoveItem(entry.Item, amountNeeded);
                if (amountNeeded <= 0) break;
            }
        }

        // Start crafting timer
        float craftingTime = selectedRecipe.CraftingTime / craftingSpeed;
        craftingTimer.Start(craftingTime);
    }

    private void OnCraftingFinished()
    {
        // Add outputs
        foreach (var entry in selectedRecipe.Outputs)
        {
            foreach (var storage in outputs)
            {
                int remaining = storage.AddItem(entry.Item, entry.Amount);
                if (remaining == 0) break; // Fully stored
            }
        }

        isCrafting = false;

        if (loopCrafting == false)
        {
            isActive = false;
        }

    }

    public void CraftNewRecipe(Recipe recipe)
    {
        if (isCrafting) return;

        // add queueing system
        
        isActive = true;
        selectedRecipe = recipe;
    }
}
