using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private Dictionary<string, List<DialogueRecord>> dialoguesCache = new Dictionary<string, List<DialogueRecord>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllDialogues(); // можно грузить сразу
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // ЗАГРУЗКА ИЗ БД
    // =========================
    private void LoadAllDialogues()
    {
        if (DatabaseManager.Instance?.Connection == null)
        {
            Debug.LogError("DatabaseManager не инициализирован. Диалоги не загружены.");
            return;
        }

        var db = DatabaseManager.Instance.Connection;
        List<DialogueRecord> allLines = db.Table<DialogueRecord>().ToList();

        dialoguesCache.Clear();

        foreach (var line in allLines)
        {
            if (!dialoguesCache.ContainsKey(line.npc_id))
                dialoguesCache[line.npc_id] = new List<DialogueRecord>();

            dialoguesCache[line.npc_id].Add(line);
        }

        Debug.Log($"DialogueManager: загружено {allLines.Count} реплик для {dialoguesCache.Count} NPC.");
    }

    public void ReloadDialogues()
    {
        LoadAllDialogues();
    }

    // =========================
    // ЗАПУСК ДИАЛОГА (НЕ МЕНЯЕМ СИГНАТУРУ!)
    // =========================
    public void StartDialogue(string npcId)
    {
        if (dialoguesCache.Count == 0)
            LoadAllDialogues();

        if (!dialoguesCache.ContainsKey(npcId))
        {
            Debug.LogWarning($"DialogueManager: нет диалогов для NPC {npcId}");
            return;
        }

        var lines = dialoguesCache[npcId]
            .OrderBy(l => l.priority)
            .ToList();

        DialogueRecord firstLine = GetFirstAvailableLine(lines);

        if (firstLine != null)
            ShowLine(firstLine);
        else
            Debug.Log($"DialogueManager: нет доступных реплик для NPC {npcId}");
    }

    // =========================
    // ВЫБОР ПЕРВОЙ ДОСТУПНОЙ РЕПЛИКИ
    // =========================
    private DialogueRecord GetFirstAvailableLine(List<DialogueRecord> lines)
    {
        foreach (var line in lines)
        {
            // если нет условия — показываем
            if (string.IsNullOrEmpty(line.trigger_flag))
                return line;

            // если есть флаг и он true
            if (FlagManager.Instance?.GetFlag(line.trigger_flag) == true)
                return line;
        }

        return null;
    }

    // =========================
    // ПОКАЗ РЕПЛИКИ + ЛОГИКА
    // =========================
    private void ShowLine(DialogueRecord line)
    {
        if (line == null)
        {
            EndDialogue();
            return;
        }

        // вывод текста
        string displayText = string.IsNullOrEmpty(line.speaker)
            ? line.text
            : $"{line.speaker}: {line.text}";

        Debug.Log(displayText);

        // =========================
        // 🔥 НОВОЕ: установка флага
        // =========================
        if (!string.IsNullOrEmpty(line.set_flag))
        {
            FlagManager.Instance?.SetFlag(line.set_flag, true);
            Debug.Log($"Установлен флаг: {line.set_flag}");
        }

        // =========================
        // ПЕРЕХОД К СЛЕДУЮЩЕЙ СТРОКЕ
        // =========================
        if (line.next_id.HasValue && line.next_id.Value > 0)
        {
            DialogueRecord nextLine = dialoguesCache[line.npc_id]
                .FirstOrDefault(l => l.id == line.next_id.Value);

            if (nextLine != null)
            {
                ShowLine(nextLine);
                return;
            }
            else
            {
                Debug.LogWarning($"DialogueManager: не найдена следующая реплика id={line.next_id}");
            }
        }

        // если нет next — диалог окончен
        EndDialogue();
    }

    // =========================
    // ЗАВЕРШЕНИЕ
    // =========================
    private void EndDialogue()
    {
        Debug.Log("Диалог завершён");
    }
}