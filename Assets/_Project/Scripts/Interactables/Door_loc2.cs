using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_loc2 : InteractableObject
{
    [Header("Required Flag")]
    [SerializeField] private string requiredFlag = "talked_to_cat_loc2";

    [Header("Next Scene")]
    [SerializeField] private string sceneToLoad = "NextLocation";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        // Если сейчас идёт диалог — не даём перейти
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог ещё идёт — переход запрещён");
            return;
        }

        bool canGoNext = FlagManager.Instance.GetFlag(requiredFlag);

        Debug.Log(
            $"Флаг {requiredFlag} = {canGoNext}"
        );

        if (canGoNext)
        {
            Debug.Log($"Переход на сцену: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}