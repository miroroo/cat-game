using UnityEngine;

/// <summary>
/// Автоматически запускает вступительное сообщение и основной диалог
/// при загрузке сцены.
/// </summary>
public class SceneDialogueStarter : MonoBehaviour
{
    [Header("Dialogue Settings")]

    [SerializeField]
    private int dialogueId = 0;     // ID основного диалога, который нужно запустить

    [SerializeField]
    private string description = "";    // Вступительное описание перед основным диалогом

    [SerializeField]
    private bool autoStartDialogue = true;  // Нужно ли автоматически запускать диалог при старте сцены

    [SerializeField]
    private float startDelay = 0.5f;   // Задержка перед запуском вступительного сообщения

    private bool dialogueStarted = false;   // Защита от повторного запуска диалога

    /// <summary>
    /// При старте сцены запускает отложенный вызов вступительного диалога,
    /// если включён автоматический запуск.
    /// </summary>
    private void Start()
    {
        if (autoStartDialogue)
        {
            Invoke(nameof(StartIntroDialogue), startDelay);
        }
    }

    /// <summary>
    /// Показывает вступительное сообщение перед основным диалогом.
    /// Если UI диалога отсутствует — сразу запускает основной диалог.
    /// </summary>
    private void StartIntroDialogue()
    {
        // Не даём запустить диалог повторно
        if (dialogueStarted)
            return;

        dialogueStarted = true;

        if (DialogueUI.Instance != null)
        {
            // Показываем вступительное сообщение,
            // после закрытия запускается основной диалог
            DialogueUI.Instance.Show(
                "",
                description,
                StartMainDialogue
            );
        }
        else
        {
            // Если UI отсутствует — сразу запускаем основной диалог
            StartMainDialogue();
        }
    }

    /// <summary>
    /// Запускает основной диалог по указанному dialogueId.
    /// </summary>
    private void StartMainDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueId);
        }
    }
}

