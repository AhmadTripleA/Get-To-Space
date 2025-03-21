using Godot;
using System;
using System.Collections.Generic;

public static class ItemRegistry
{
    public static event Action OnItemsLoaded;
    public static Dictionary<int, Item> Items = [];

    public static void LoadItems()
    {
        Items.Clear();

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
                        Items[item.Id] = item;
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

    public static Item GetItemById(int id)
    {
        return Items.TryGetValue(id, out var item) ? item : null;
    }
}
