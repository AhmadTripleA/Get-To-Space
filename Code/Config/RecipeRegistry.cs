using Godot;
using System.Collections.Generic;
using System.Text.Json;

public class RecipeData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IconPath { get; set; }
    public float CraftingTime { get; set; }
}

public static class RecipeRegistry
{
    private static Dictionary<int, Recipe> _recipes = new();

    public static void LoadIRecipes(string jsonFilePath)
    {
        // string jsonText = FileAccess.GetFileAsString(jsonFilePath);
        // if (string.IsNullOrEmpty(jsonText))
        // {
        //     GD.PrintErr("RecipeRegistry: Failed to load Recipes JSON!");
        //     return;
        // }

        // // Deserialize JSON into an array of Recipe data
        // var Recipes = JsonSerializer.Deserialize<List<RecipeData>>(jsonText);
        // if (Recipes == null)
        // {
        //     GD.PrintErr("RecipeRegistry: Failed to parse Recipes JSON!");
        //     return;
        // }

        // // Convert Recipe data into actual Recipe objects and store them
        // foreach (var RecipeData in Recipes)
        // {
        //     var recipe = new Recipe(RecipeData.Id, RecipeData.Name, RecipeData.IconPath, RecipeData.CraftingTime);
        //     _recipes[recipe.Id] = recipe;
        // }

        // GD.Print($"RecipeRegistry: Loaded {_recipes.Count} Recipes.");
    }

    public static Recipe GetRecipeById(int id)
    {
        return _recipes.TryGetValue(id, out var recipe) ? recipe : null;
    }

    public static Recipe GetRecipeByName(string name)
    {
        foreach (var recipe in _recipes.Values)
        {
            if (recipe.Name == name)
                return recipe;
        }
        return null;
    }
}
