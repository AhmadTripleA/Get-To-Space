using Godot.Collections;
using Godot;

public enum ResearchStatus { Locked, Unlocked, InProgress, Finished }

public partial class Research : Resource
{
    //
    // @TODO: handle research cost and requirements
    //

    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public bool IsUnlocked { get; set; }
    [Export] public Item[] UnlockedItems { get; set; } = [];
    [Export] public Recipe[] UnlockedRecipes { get; set; } = [];
    [Export] public Dictionary<int, int> ItemCosts { get; set; }
    [Export] public int[] RequiredResearchIds { get; set; } = [];
}