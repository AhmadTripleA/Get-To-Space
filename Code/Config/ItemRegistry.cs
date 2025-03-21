using Godot;
using System.Collections.Generic;
using System.Text.Json;

public class ItemData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IconPath { get; set; }
}

public static class ItemRegistry
{
    private static Dictionary<int, Item> _items = new();

    public static void LoadItems(string jsonFilePath)
    {
        string jsonText = FileAccess.GetFileAsString(jsonFilePath);
        if (string.IsNullOrEmpty(jsonText))
        {
            GD.PrintErr("ItemRegistry: Failed to load items JSON!");
            return;
        }

        // Deserialize JSON into an array of item data
        var items = JsonSerializer.Deserialize<List<ItemData>>(jsonText);
        if (items == null)
        {
            GD.PrintErr("ItemRegistry: Failed to parse items JSON!");
            return;
        }

        // Convert item data into actual Item objects and store them
        foreach (var itemData in items)
        {
            var item = new Item(itemData.Id, itemData.Name, itemData.IconPath);
            _items[item.Id] = item;
        }

        GD.Print($"ItemRegistry: Loaded {_items.Count} items.");
    }

    public static Item GetItemById(int id)
    {
        return _items.TryGetValue(id, out var item) ? item : null;
    }

    public static Item GetItemByName(string name)
    {
        foreach (var item in _items.Values)
        {
            if (item.Name == name)
                return item;
        }
        return null;
    }
}
