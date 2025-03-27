using Godot;
using System;
using System.Collections.Generic;

public static class RecipeDB
{
    public static event Action OnRecipesLoaded;
    private static Dictionary<int, Recipe> registry = [];

    public static void LoadAll()
    {
        registry.Clear();

        string RecipeFolder = "res://Assets/Recipes/";
        var dir = DirAccess.Open(RecipeFolder);

        if (dir != null)
        {
            dir.ListDirBegin();

            string fileName;
            while ((fileName = dir.GetNext()) != "")
            {
                if (fileName.EndsWith(".tres"))
                {
                    string filePath = RecipeFolder + fileName;
                    Recipe recipe = ResourceLoader.Load<Recipe>(filePath);

                    if (recipe != null)
                    {
                        registry[recipe.Id] = recipe;
                    }
                    else
                    {
                        GD.PrintErr($"Failed to load recipe: {filePath}");
                    }
                }
            }
            OnRecipesLoaded?.Invoke();
        }
        else
        {
            GD.PrintErr($"Failed to open Recipe directory: {RecipeFolder}");
        }
    }

    public static Recipe GetById(int id)
    {
        return registry.TryGetValue(id, out var recipe) ? recipe : null;
    }

    public static Recipe GetByName(string name)
    {
        foreach (var recipe in registry.Values)
        {
            if (recipe.Name == name)
                return recipe;
        }
        return null;
    }

    public static Recipe[] GetAll()
    {
        return [.. registry.Values];
    }

    public static bool Unlock(int id)
    {
        if (registry.TryGetValue(id, out var recipe))
        {
            recipe.IsUnlocked = true;
            return true;
        }
        return false;
    }
}
