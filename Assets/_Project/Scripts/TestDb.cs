using UnityEngine;
using System.Collections;

public class TestManagers : MonoBehaviour
{
    [Header("Тестовые параметры")]
    [SerializeField] private string testObjectId = "object_1";      // ID объекта для ActionManager
    [SerializeField] private string testNpcId = "cat";           // ID NPC для DialogueManager
    [SerializeField] private int testItemId = 1;                   // ID предмета для инвентаря
    [SerializeField] private string testSceneName = "MainMenu";    // Сцена для загрузки (осторожно!)

    private void Start()
    {
        // Запускаем тесты с небольшой задержкой, чтобы все менеджеры успели инициализироваться
        StartCoroutine(RunTests());
    }

    private IEnumerator RunTests()
    {
        yield return new WaitForSeconds(1f); // Ждём 1 секунду

        Debug.Log("===== НАЧАЛО ТЕСТИРОВАНИЯ МЕНЕДЖЕРОВ =====\n");

        // 1. Проверка ItemDatabase
        Debug.Log("--- Тест ItemDatabase ---");
        Item item = ItemDatabase.GetItem(testItemId);
        if (item != null)
        {
            Debug.Log($"✅ Предмет ID {testItemId} загружен: {item.itemName}");
        }
        else
        {
            Debug.LogError($"❌ Предмет с ID {testItemId} не найден!");
        }

        // 2. Проверка InventoryManager
        Debug.Log("\n--- Тест InventoryManager ---");
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("❌ InventoryManager не инициализирован!");
        }
        else
        {
            // Текущее содержимое
            Debug.Log($"Текущее количество предметов в инвентаре: {InventoryManager.Instance.items.Count}");

            // Добавляем предмет
            if (item != null)
            {
                InventoryManager.Instance.AddItem(item);
                Debug.Log($"✅ Добавлен предмет {item.itemName}. Теперь предметов: {InventoryManager.Instance.items.Count}");
            }

            // Пытаемся удалить предмет
            InventoryManager.Instance.RemoveItem(testItemId);
            Debug.Log($"✅ Предмет ID {testItemId} удалён. Теперь предметов: {InventoryManager.Instance.items.Count}");

        }

        // 3. Проверка FlagManager
        Debug.Log("\n--- Тест FlagManager ---");
        if (FlagManager.Instance == null)
        {
            Debug.LogError("❌ FlagManager не инициализирован!");
        }
        else
        {
            string testFlag = "talked_to_cat";
            bool initialValue = FlagManager.Instance.GetFlag(testFlag);
            Debug.Log($"Начальное значение флага '{testFlag}': {initialValue}");

            FlagManager.Instance.SetFlag(testFlag, true);
            bool newValue = FlagManager.Instance.GetFlag(testFlag);
            Debug.Log($"После установки в true: {newValue}");

            FlagManager.Instance.SetFlag(testFlag, false);
            Debug.Log($"После сброса в false: {FlagManager.Instance.GetFlag(testFlag)}");
        }

        // 4. Проверка ActionManager
        Debug.Log("\n--- Тест ActionManager ---");
        if (ActionManager.Instance == null)
        {
            Debug.LogError("❌ ActionManager не инициализирован!");
        }
        else
        {
            Debug.Log($"Пытаемся выполнить действие для объекта '{testObjectId}'...");
            ActionManager.Instance.ExecuteAction(testObjectId);
            // Действие выполнится, но результат увидим по логам внутри ActionManager
            Debug.Log("✅ ExecuteAction вызван (проверьте логи выше)");
        }

        // 5. Проверка DialogueManager
        Debug.Log("\n--- Тест DialogueManager ---");
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("❌ DialogueManager не инициализирован!");
        }
        else
        {
            Debug.Log($"Пытаемся начать диалог с NPC '{testNpcId}'...");
            DialogueManager.Instance.StartDialogue(testNpcId);
            Debug.Log("✅ StartDialogue вызван (проверьте логи диалога)");
        }

        // 6. Проверка SceneLoader (закомментировано, чтобы случайно не переключить сцену)
        Debug.Log("\n--- Тест SceneLoader ---");
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("❌ SceneLoader не инициализирован!");
        }
        else
        {
            Debug.Log($"SceneLoader готов. Для загрузки сцены '{testSceneName}' раскомментируйте код.");
            // SceneLoader.Instance.LoadLocation(testSceneName); // Раскомментируйте для реальной проверки
        }

        Debug.Log("\n===== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО =====\n");
    }
}