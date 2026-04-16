using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-80)]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private Dictionary<string, List<DialogueRecord>> dialoguesCache = new Dictionary<string, List<DialogueRecord>>();
    private Dictionary<int, DialogueRecord> dialogueById = new Dictionary<int, DialogueRecord>();
    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllDialogues();
            //if (FlagManager.Instance != null)
            //{
            //    FlagManager.Instance.ResetAllFlags();
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string GetProgressKey()
    {
        return "dialogue_progress";
    }

    private string GetCompletedKey()
    {
        return "dialogue_completed";
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
        dialogueById.Clear();


        foreach (var line in allLines)
        {
            dialogueById[line.id] = line;

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


    public void StartDialogue(string npcId)
    {
        if (IsDialogueActive) return;

        if (!dialoguesCache.ContainsKey(npcId))
            LoadAllDialogues();

        if (!dialoguesCache.ContainsKey(npcId))
        {
            Debug.LogWarning($"Нет диалогов для {npcId}");
            return;
        }

        var lines = dialoguesCache[npcId];

        int lastId = FlagManager.Instance.GetInt(GetProgressKey());

        DialogueRecord current;

        if (lastId == 0)
        {
            // ищем начало всей цепочки
            current = dialogueById.Values
                .FirstOrDefault(l => !dialogueById.Values.Any(x => x.next_id == l.id));
        }
        else
        {
            dialogueById.TryGetValue(lastId, out current);
        }

        RunDialogueChain(current);
    }

    private void RunDialogueChain(DialogueRecord startLine)
    {
        ShowLine(startLine);
    }

    private void ShowLine(DialogueRecord line)
    {
        if (line == null)
        {
            EndDialogue();
            return;
        }

        IsDialogueActive = true;

        Debug.Log("DialogueUI instance: " + DialogueUI.Instance);

        string speaker = string.IsNullOrEmpty(line.speaker) ? "" : line.speaker;
        string text = line.text;

        Debug.Log("TEXT: " + text);
        // Показываем UI и ждём кнопку
        DialogueUI.Instance.Show(speaker, text, () =>
        {
            // ставим флаг
            if (!string.IsNullOrEmpty(line.set_flag))
            {
                FlagManager.Instance?.SetFlag(line.set_flag, true);
            }

            ShowNextLine(line);
        });
    }

    private void ShowNextLine(DialogueRecord currentLine)
    {
        FlagManager.Instance.SetInt(GetProgressKey(), currentLine.id);
        Debug.Log($"SAVE PROGRESS: {currentLine.id}");

        if (currentLine.next_id.HasValue && currentLine.next_id.Value > 0)
        {
            if (dialogueById.TryGetValue(currentLine.next_id.Value, out var nextLine))
            {
                ShowLine(nextLine);
                return;
            }
        }

        EndDialogue(currentLine);
    }

    private void EndDialogue(DialogueRecord lastLine = null)
    {
        DialogueUI.Instance.Hide();
        IsDialogueActive = false;

        if (lastLine != null)
        {
            string key = GetCompletedKey();
            FlagManager.Instance.SetFlag(key, true);
        }

        Debug.Log("Диалог завершён");
    }
}