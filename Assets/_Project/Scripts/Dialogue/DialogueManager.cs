using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[DefaultExecutionOrder(-80)]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // Все реплики по id
    private Dictionary<int, DialogueRecord> dialogueById =
        new Dictionary<int, DialogueRecord>();
    private Action onDialogueComplete;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadAllDialogues();
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

        dialogueById.Clear();

        foreach (var line in allLines)
        {
            dialogueById[line.id] = line;
        }

        Debug.Log($"DialogueManager: загружено {allLines.Count} реплик.");
    }

    public void ReloadDialogues()
    {
        LoadAllDialogues();
    }

    /// <summary>
    /// Запуск диалога с конкретного стартового узла
    /// </summary>
    public void StartDialogue(int startId, Action completeCallback = null)
    {
        if (IsDialogueActive)
            return;

        onDialogueComplete = completeCallback;

        if (dialogueById.Count == 0)
            LoadAllDialogues();

        if (!dialogueById.TryGetValue(startId, out var startLine))
        {
            Debug.LogWarning($"Диалог с startId = {startId} не найден");
            return;
        }

        RunDialogueChain(startLine);
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

        string speaker = string.IsNullOrEmpty(line.speaker)
            ? ""
            : line.speaker;

        string text = line.text;

        Debug.Log($"SHOW LINE: {line.id} | {speaker}: {text}");

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Show(speaker, text, () =>
            {
                // Ставим флаг после показа реплики
                if (!string.IsNullOrEmpty(line.set_flag))
                {
                    FlagManager.Instance?.SetFlag(line.set_flag, true);
                    Debug.Log($"SET FLAG: {line.set_flag}");
                }

                ShowNextLine(line);
            });
        }
        
    }

    private void ShowNextLine(DialogueRecord currentLine)
    {
        if (currentLine.next_id.HasValue &&
            currentLine.next_id.Value > 0)
        {
            if (dialogueById.TryGetValue(
                currentLine.next_id.Value,
                out var nextLine))
            {
                ShowLine(nextLine);
                return;
            }
        }

        EndDialogue(currentLine);
    }

    private void EndDialogue(DialogueRecord lastLine = null)
    {
        IsDialogueActive = false;

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Hide();
        }

        Debug.Log("Диалог завершён");

        onDialogueComplete?.Invoke();
        onDialogueComplete = null;
    }
}


