using Godot;
using System;
using System.Collections.Generic;

public static class ItemDB
{
    public static event Action OnItemsLoaded;
    private static Dictionary<int, Item> registry = [];

    public static void LoadAll()
    {
        registry.Clear();

        string itemsFolder = "res://Assets/Items/";
        var dir = DirAccess.Open(itemsFolder);

        if (dir != null)
        {
            dir.ListDirBegin();

            string fileName;
            while ((fileName = dir.GetNext()) != "")
            {
                if (fileName.EndsWith(".tres"))
                {
                    string filePath = itemsFolder + fileName;
                    Item item = ResourceLoader.Load<Item>(filePath);

                    if (item != null)
                    {
                        registry[item.Id] = item;
                        GD.Print($"Loaded item: {item.Name} (ID: {item.Id})");
                    }
                    else
                    {
                        GD.PrintErr($"Failed to load item: {filePath}");
                    }
                }
            }
            OnItemsLoaded?.Invoke();
        }
        else
        {
            GD.PrintErr($"Failed to open items directory: {itemsFolder}");
        }
    }

    public static Item GetById(int id)
    {
        return registry.TryGetValue(id, out var item) ? item : null;
    }

    public static Item[] GetAll()
    {
        return [.. registry.Values];
    }

    public static bool Unlock(int id)
    {
        if (registry.TryGetValue(id, out var item))
        {
            item.IsUnlocked = true;
            return true;
        }
        return false;
    }
}
