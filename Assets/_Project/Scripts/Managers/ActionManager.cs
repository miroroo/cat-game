using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

/// <summary>
/// Менеджер действий. Загружает все действия из БД и выполняет подходящее действие при взаимодействии с объектом.
/// </summary>
public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }

    // Кэш действий: ключ — objectId, значение — список действий для этого объекта
    private Dictionary<string, List<ActionData>> actionsCache = new Dictionary<string, List<ActionData>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllActions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Загружает все действия из таблицы Actions в кэш.
    /// </summary>
    private void LoadAllActions()
    {
        if (DatabaseManager.Instance?.Connection == null)
        {
            Debug.LogError("DatabaseManager не инициализирован. Действия не загружены.");
            return;
        }

        var db = DatabaseManager.Instance.Connection;
        List<ActionData> allActions = db.Table<ActionData>().ToList();

        actionsCache.Clear();
        foreach (var action in allActions)
        {
            if (!actionsCache.ContainsKey(action.objectId))
                actionsCache[action.objectId] = new List<ActionData>();
            actionsCache[action.objectId].Add(action);
        }

        Debug.Log($"ActionManager: загружено {allActions.Count} действий для {actionsCache.Count} объектов.");
    }

    /// <summary>
    /// Выполняет подходящее действие для указанного объекта.
    /// Перебирает действия по приоритету, проверяет условия (флаг и предмет).
    /// Первое подходящее действие выполняется.
    /// </summary>
    /// <param name="objectId">Идентификатор объекта</param>
    public void ExecuteAction(string objectId)
    {
        if (!actionsCache.ContainsKey(objectId))
        {
            Debug.Log($"ActionManager: нет действий для объекта {objectId}");
            return;
        }

        var actions = actionsCache[objectId].OrderBy(a => a.priority).ToList();

        foreach (var action in actions)
        {
            bool conditionsMet = true;

            // Проверка флага (если указан)
            if (!string.IsNullOrEmpty(action.conditionFlag))
            {
                conditionsMet = FlagManager.Instance != null &&
                                FlagManager.Instance.GetFlag(action.conditionFlag);
            }
            if (conditionsMet)
            {
                PerformAction(action);
                return; // Выполнили первое подходящее действие
            }
        }

        // Если ни одно действие не подошло, можно показать стандартное сообщение
        Debug.Log($"ActionManager: ни одно действие не подошло для объекта {objectId}");
        // Здесь можно вызвать UIManager.Instance.ShowMessage("...");
    }

    /// <summary>
    /// Выполняет конкретное действие (тип + значение).
    /// </summary>
    private void PerformAction(ActionData action)
    {
        Debug.Log($"ActionManager: выполняю действие {action.actionType} = {action.actionValue} (объект {action.objectId})");

        switch (action.actionType)
        {
            case "set_flag":
                if (FlagManager.Instance != null)
                    FlagManager.Instance.SetFlag(action.actionValue, true);
                else
                    Debug.LogError("FlagManager не найден");
                break;

            case "add_item":
                if (int.TryParse(action.actionValue, out int addId))
                {
                    Item item = ItemDatabase.GetItem(addId);
                    if (item != null && InventoryManager.Instance != null)
                        InventoryManager.Instance.AddItem(item);
                    else
                        Debug.LogWarning($"Не удалось добавить предмет ID {addId}");
                }
                else
                {
                    Debug.LogError($"Неверный формат ID для add_item: {action.actionValue}");
                }
                break;

            case "remove_item":
                if (int.TryParse(action.actionValue, out int remId))
                {
                    if (InventoryManager.Instance != null)
                        InventoryManager.Instance.RemoveItem(remId);
                }
                else
                {
                    Debug.LogError($"Неверный формат ID для remove_item: {action.actionValue}");
                }
                break;

            case "show_message":
                // Здесь можно вызвать UIManager для отображения сообщения
                Debug.Log("Сообщение: " + action.actionValue);
                // UIManager.Instance?.ShowMessage(action.actionValue);
                break;

            case "load_scene":
                if (SceneLoader.Instance != null)
                    SceneLoader.Instance.LoadLocation(action.actionValue);
                else
                    Debug.LogError("SceneLoader не найден");
                break;

            case "start_dialogue":
                if (DialogueManager.Instance != null)
                    DialogueManager.Instance.StartDialogue(action.actionValue);
                else
                    Debug.LogError("DialogueManager не найден");
                break;

            // Добавьте другие типы действий по необходимости
            default:
                Debug.LogWarning($"ActionManager: неизвестный тип действия {action.actionType}");
                break;
        }
    }

    /// <summary>
    /// Очищает кэш и перезагружает действия (полезно при горячей перезагрузке или изменении БД).
    /// </summary>
    public void ReloadActions()
    {
        LoadAllActions();
    }
}