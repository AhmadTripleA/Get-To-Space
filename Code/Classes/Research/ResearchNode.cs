using Godot;

public enum ResearchStatus { Locked, Unlocked, InProgress, Finished }

public partial class ResearchNode
{
    [Export] public string Label { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public ResearchStatus Status { get; set; }
}