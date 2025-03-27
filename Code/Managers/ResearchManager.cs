using Godot;
using System;

public partial class ResearchManager : Node

{
    public static event Action OnResearchNodesLoaded;
    [Signal] public delegate void ResearchUnlockedEventHandler(int researchId);


    // Called when research is completed
    public void Unlock(Research research)
    {
        if (research == null) return;
        
        EmitSignal(SignalName.ResearchUnlocked, research.Id);

        foreach (Item item in research.UnlockedItems)
        {
            ItemDB.Unlock(item.Id);
        }

        foreach (Recipe recipe in research.UnlockedRecipes)
        {
            RecipeDB.Unlock(recipe.Id);
        }
    }
}