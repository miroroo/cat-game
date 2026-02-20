using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> items = new List<Item>(); // список предметов

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

    // Метод для добавления предмета
    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        Debug.Log("Предмет добавлен: " + newItem.itemName);
    }

    // Метод для проверки, есть ли предмет
    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    // Метод для удаления предмета (если понадобится)
    public void RemoveItem(string itemName)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
            items.Remove(item);
    }
}