using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDatabase
{
    private static Dictionary<int, Item> itemsById;

    public static void LoadAllItems()
    {

        if (DatabaseManager.Instance == null || DatabaseManager.Instance.Connection == null)
        {
            Debug.LogError("DatabaseManager не готов, предметы не загружены");
            return;
        }

        itemsById = new Dictionary<int, Item>();

        // Получаем соединение из DatabaseManager
        var db = DatabaseManager.Instance.Connection;

        // Загружаем ВСЕ предметы одной строкой!
        // db.Table<Item>() возвращает все записи из таблицы Item
        List<Item> allItems = db.Table<Item>().ToList();

        foreach (var item in allItems)
        {
            itemsById[item.id] = item;
        }

        Debug.Log($"Загружено {itemsById.Count} предметов из БД");
    }

    public static Item GetItem(int id)
    {
        itemsById.TryGetValue(id, out Item item);
        return item;
    }

    public static List<Item> GetItemsByLocation(int locationId)
    {
        // LINQ-запрос прямо в коде!
        return itemsById.Values
            .Where(item => item.locationId == locationId)
            .ToList();
    }
}