using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> items = new List<Item>(); // предметы в инвентаре

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadInventoryFromDB();
    }

    private void LoadInventoryFromDB()
    {
        items.Clear();
        var db = DatabaseManager.Instance.Connection;

        // Загружаем все слоты инвентаря
        List<InventorySlot> slots = db.Table<InventorySlot>()
            .OrderBy(s => s.slotIndex)
            .ToList();

        foreach (var slot in slots)
        {
            Item item = ItemDatabase.GetItem(slot.itemId);
            if (item != null)
            {
                items.Add(item);
            }
            else
            {
                Debug.LogWarning($"Предмет с ID {slot.itemId} не найден в базе");
            }
        }

        Debug.Log($"Инвентарь загружен: {items.Count} предметов");
    }

    public void AddItem(Item newItem)
    {
        if (newItem == null) return;
        items.Add(newItem);

        var db = DatabaseManager.Instance.Connection;

        // Создаём новый слот инвентаря
        var slot = new InventorySlot
        {
            itemId = newItem.id,
            slotIndex = items.Count - 1
        };

        db.Insert(slot);
        Debug.Log("Предмет добавлен: " + newItem.itemName);
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    public void RemoveItem(string itemName)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            items.Remove(item);

            var db = DatabaseManager.Instance.Connection;

            // Удаляем все слоты с этим itemId (обычно один)
            db.Table<InventorySlot>()
                .Delete(s => s.itemId == item.id);
        }
    }

    public void RemoveItem(int itemId)
    {
        Item item = items.Find(i => i.id == itemId);
        if (item != null)
        {
            items.Remove(item);

            var db = DatabaseManager.Instance.Connection;
            db.Table<InventorySlot>()
                .Delete(s => s.itemId == itemId);
        }
    }
}