using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class RecipeDB
{
    public static event Action OnRecipesLoaded;
    private static Dictionary<int, Recipe> _recipies = [];

    public static void LoadRecipes()
    {
        _recipies.Clear();

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
                        _recipies[recipe.Id] = recipe;
                        GD.Print($"Loaded recipe: {recipe.Name} (ID: {recipe.Id})");
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

    public static Recipe GetRecipeById(int id)
    {
        return _recipies.TryGetValue(id, out var recipe) ? recipe : null;
    }

    public static Recipe GetRecipeByName(string name)
    {
        foreach (var recipe in _recipies.Values)
        {
            if (recipe.Name == name)
                return recipe;
        }
        return null;
    }

    public static Recipe[] GetAllRecipes(){
        return _recipies.Values.ToArray();
    }
}
