using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Настройки двери")]
    [SerializeField] private string sceneToLoad = "Coridor";
    [SerializeField] private string requiredFlag = "";
    [SerializeField] private string lockedMessage = "Дверь закрыта";
    [SerializeField] private string unlockedMessage = "Дверь открыта";

    private bool isProcessing = false;

    private void OnCollisionStay2D(Collision2D collision)
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

            Invoke(nameof(LoadNextScene), 0.2f);
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

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void ResetProcessing()
    {
        isProcessing = false;
    }
}