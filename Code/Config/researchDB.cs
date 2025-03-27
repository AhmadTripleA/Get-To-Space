using Godot;
using System;
using System.Collections.Generic;

public static class ResearchDB
{
    public static event Action OnResearchNodesLoaded;
    private static Dictionary<int, Research> registry = [];

    public static void LoadAll()
    {
        // 1. Get all files in the folder
        string researchFolder = "res://Assets/Research/";
        using var dir = DirAccess.Open(researchFolder);

        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName;
            while ((fileName = dir.GetNext()) != "")
            {
                if (!fileName.EndsWith(".tres")) continue; // Skip non-resource files

                // 2. Load each ResearchNode individually
                string filePath = $"{researchFolder}/{fileName}";
                Research res = GD.Load<Research>(filePath);

                if (res != null)
                {
                    registry[res.Id] = res; // Add to dictionary
                    GD.Print($"Loaded research: {res.Name} (ID: {res.Id})");
                }
                else
                {
                    GD.PrintErr($"Failed to load: {filePath}");
                }
            }
            OnResearchNodesLoaded?.Invoke();
        }
        else
        {
            GD.PrintErr($"Failed to open directory: {researchFolder}");
        }
    }

    public static Research GetById(int id)
    {
        return registry.TryGetValue(id, out var item) ? item : null;
    }

    public static Research[] GetAll()
    {
        return [.. registry.Values];
    }

}
