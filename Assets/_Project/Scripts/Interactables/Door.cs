using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс двери для перехода между сценами.
/// Проверяет флаг доступа и загружает новую локацию.
/// </summary>
public class Door : MonoBehaviour
{
    [Header("Настройки двери")]
    [SerializeField] private string sceneToLoad = "Coridor";
    [SerializeField] private string requiredFlag = "";
    [SerializeField] private string lockedMessage = "Дверь закрыта";
    [SerializeField] private string unlockedMessage = "Дверь открыта";

    // Защита от повторного срабатывания
    private bool isProcessing = false;

    /// <summary>
    /// Проверяет вход игрока в зону двери,
    /// показывает сообщение и выполняет переход.
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Герой столкнулся со стеной.");

        if (isProcessing)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        // Не открываем дверь во время активного диалога
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
            return;

        bool hasRequiredFlag =
            string.IsNullOrEmpty(requiredFlag) ||
            FlagManager.Instance.GetFlag(requiredFlag);

        if (hasRequiredFlag)
        {
            isProcessing = true;

            if (!string.IsNullOrEmpty(unlockedMessage))
            {
                DialogueUI.Instance.Message("", unlockedMessage, null);
            }

            Invoke(nameof(LoadNextScene), 1f);
        }
        else
        {
            isProcessing = true;

            if (!string.IsNullOrEmpty(lockedMessage))
            {
                DialogueUI.Instance.Message("", lockedMessage, null);
            }

            Invoke(nameof(ResetProcessing), 1f);
        }
    }

    /// <summary>
    /// Загружает следующую сцену через SceneLoader.
    /// </summary>
    private void LoadNextScene()
    {
        SceneLoader.Instance.LoadLocation(sceneToLoad);
    }

    /// <summary>
    /// Сбрасывает блокировку повторного срабатывания двери.
    /// </summary>
    private void ResetProcessing()
    {
        isProcessing = false;
    }
}