using UnityEngine;

public class TestDB : MonoBehaviour
{
    void Start()
    {
        // Проверим, загрузились ли предметы
        Item item = ItemDatabase.GetItem(1);
        if (item != null)
        {
            Debug.Log($"Предмет загружен: {item.itemName}, описание: {item.description}, путь к спрайту: {item.spritePath}");
        }
        else
        {
            Debug.LogError("Предмет с ID 1 не найден!");
        }

        // Проверим инвентарь (пока пуст)
        Debug.Log($"Инвентарь содержит {InventoryManager.Instance.items.Count} предметов");
    }
}