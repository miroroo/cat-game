using UnityEngine;
using System.Collections;

public class TestManagers : MonoBehaviour
{
    [Header("Тестовые параметры")]
    [SerializeField] private int testItemId = 1;                   // ID предмета для инвентаря
    [SerializeField] private string testSceneName = "LoadingScene";    // Сцена для загрузки (осторожно!)

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

        // 5. Проверка DialogueManager
        Debug.Log("\n--- Тест DialogueManager ---");
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("❌ DialogueManager не инициализирован!");
        }
        else
        {
            Debug.Log($"Пытаемся начать диалог 1...");
            DialogueManager.Instance.StartDialogue(1);
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