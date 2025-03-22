using System;
using Godot;

public partial class Mineable : Node3D
{
    [Export] public Item item = null;
    [Export] public float maxAmount = 100f;
    [Export] public float respawnCooldown = 5f; // Increased cooldown for testing
    public bool isAlive = true;

    private float resistenceFactor = 1f;
    private float amount;
    private Timer respawnTimer;

    public override void _Ready()
    {
        amount = maxAmount;

        // Initialize Timer and add it to the scene
        respawnTimer = new Timer();
        AddChild(respawnTimer);
        respawnTimer.OneShot = true;
        respawnTimer.Timeout += Respawn;
    }

    public float Mine(float strength)
    {
        if (!isAlive) return 0; // Prevent mining if it's depleted

        float minedAmount = Math.Min(strength / resistenceFactor, amount);
        amount -= minedAmount;

        if (amount <= 0) Deplete();

        return minedAmount;
    }

    private void Deplete()
    {
        isAlive = false;
        Visible = false; // Hide the node visually
        TriggerRespawn();
    }

    private void TriggerRespawn()
    {
        respawnTimer.Start(respawnCooldown);
    }

    private void Respawn()
    {
        isAlive = true;
        amount = maxAmount;
        Visible = true; // Show the node again
    }
}
