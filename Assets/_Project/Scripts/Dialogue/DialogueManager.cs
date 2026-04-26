using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

/// <summary>
/// Управляет загрузкой, запуском и последовательным показом диалогов.
/// Получает данные из базы, хранит реплики и передаёт их в DialogueUI.
/// </summary>
[DefaultExecutionOrder(-80)]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private Dictionary<int, DialogueRecord> dialogueById =
        new Dictionary<int, DialogueRecord>();   // Словарь всех реплик:  key = id реплики, value = сама реплика.

    private Action onDialogueComplete;  // Действие, которое выполняется после завершения диалога.

    /// <summary>
    /// Показывает, активен ли сейчас диалог.
    /// Используется для защиты от повторного запуска.
    /// </summary>
    public bool IsDialogueActive { get; private set; }

    /// <summary>
    /// Singleton-инициализация и первая загрузка диалогов из базы данных.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Сохраняем объект между сценами
            DontDestroyOnLoad(gameObject);

            // Загружаем все диалоги при старте
            LoadAllDialogues();
        }
        else
        {
            // Удаляем дубликат объекта
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Загружает все реплики из таблицы DialogueRecord
    /// и сохраняет их в словарь для быстрого доступа по id.
    /// </summary>
    private void LoadAllDialogues()
    {
        if (DatabaseManager.Instance?.Connection == null)
        {
            Debug.LogError(
                "DatabaseManager не инициализирован. Диалоги не загружены."
            );
            return;
        }

        var db = DatabaseManager.Instance.Connection;

        // Получаем все строки из таблицы диалогов
        List<DialogueRecord> allLines =
            db.Table<DialogueRecord>().ToList();

        dialogueById.Clear();

        // Заполняем словарь по id
        foreach (var line in allLines)
        {
            dialogueById[line.id] = line;
        }

        Debug.Log(
            $"DialogueManager: загружено {allLines.Count} реплик."
        );
    }

    /// <summary>
    /// Принудительно перезагружает диалоги из базы данных.
    /// Полезно при обновлении данных во время игры.
    /// </summary>
    public void ReloadDialogues()
    {
        LoadAllDialogues();
    }

    /// <summary>
    /// Запускает диалог с указанной стартовой реплики.
    /// Можно передать callback, который выполнится после завершения.
    /// </summary>
    public void StartDialogue(
        int startId,
        Action completeCallback = null
    )
    {
        // Если диалог уже идёт — новый не запускаем
        if (IsDialogueActive)
            return;

        onDialogueComplete = completeCallback;

        // Если словарь пуст — пробуем загрузить заново
        if (dialogueById.Count == 0)
            LoadAllDialogues();

        // Ищем стартовую реплику
        if (!dialogueById.TryGetValue(startId, out var startLine))
        {
            Debug.LogWarning(
                $"Диалог с startId = {startId} не найден"
            );
            return;
        }

        RunDialogueChain(startLine);
    }

    /// <summary>
    /// Запускает цепочку диалога с первой найденной реплики.
    /// </summary>
    private void RunDialogueChain(DialogueRecord startLine)
    {
        ShowLine(startLine);
    }

    /// <summary>
    /// Показывает одну реплику через DialogueUI.
    /// После завершения автоматически переходит к следующей.
    /// </summary>
    private void ShowLine(DialogueRecord line)
    {
        if (line == null)
        {
            EndDialogue();
            return;
        }

        IsDialogueActive = true;

        // Если имя говорящего пустое — показываем без имени
        string speaker = string.IsNullOrEmpty(line.speaker)
            ? ""
            : line.speaker;

        string text = line.text;

        Debug.Log(
            $"SHOW LINE: {line.id} | {speaker}: {text}"
        );

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Show(
                speaker,
                text,
                () =>
                {
                    // После показа реплики можем установить флаг
                    if (!string.IsNullOrEmpty(line.set_flag))
                    {
                        FlagManager.Instance?.SetFlag(
                            line.set_flag,
                            true
                        );

                        Debug.Log(
                            $"SET FLAG: {line.set_flag}"
                        );
                    }

                    // Переход к следующей реплике
                    ShowNextLine(line);
                }
            );
        }
    }

    /// <summary>
    /// Ищет следующую реплику по next_id.
    /// Если следующей реплики нет — завершает диалог.
    /// </summary>
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

        // Если следующая реплика не найдена
        EndDialogue(currentLine);
    }

    /// <summary>
    /// Завершает диалог:
    /// скрывает UI, сбрасывает состояние и вызывает callback.
    /// </summary>
    private void EndDialogue(DialogueRecord lastLine = null)
    {
        IsDialogueActive = false;

        // Скрываем UI диалога
        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Hide();
        }

        Debug.Log("Диалог завершён");

        // Выполняем callback после завершения
        onDialogueComplete?.Invoke();
        onDialogueComplete = null;
    }
}
