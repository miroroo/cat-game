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
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void StartDialogue(string npcId)
    {
        if (dialoguesCache.Count == 0)
            LoadAllDialogues();

        if (!dialoguesCache.ContainsKey(npcId))
        {
            Debug.LogWarning($"DialogueManager: нет диалогов для NPC {npcId}");
            return;
        }

        var lines = dialoguesCache[npcId].OrderBy(l => l.priority).ToList();
        DialogueRecord firstLine = GetFirstAvailableLine(lines);
        if (firstLine != null)
            ShowLine(firstLine);
        else
            Debug.Log($"DialogueManager: нет доступных реплик для NPC {npcId}");
    }

    private DialogueRecord GetFirstAvailableLine(List<DialogueRecord> lines)
    {
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line.trigger_flag))
                return line;

            if (FlagManager.Instance != null && FlagManager.Instance.GetFlag(line.trigger_flag))
                return line;
        }
        return null;
    }

    private void ShowLine(DialogueRecord line)
    {
        string displayText = string.IsNullOrEmpty(line.speaker) ? line.text : $"{line.speaker}: {line.text}";
        Debug.Log(displayText);

        if (line.next_id.HasValue && line.next_id.Value > 0)
        {
            var nextLine = dialoguesCache[line.npc_id].FirstOrDefault(l => l.id == line.next_id.Value);
            if (nextLine != null)
                ShowLine(nextLine);
            else
            {
                Debug.LogWarning($"DialogueManager: не найдена следующая реплика с id {line.next_id} для NPC {line.npc_id}");
                EndDialogue();
            }
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Диалог завершён");
    }

    public void ReloadDialogues()
    {
        LoadAllDialogues();
    }
}
