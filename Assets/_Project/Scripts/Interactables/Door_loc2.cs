using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door_loc2 : InteractableObject
{
    [Header("Required Flag")]
    [SerializeField] private string requiredFlag = "talked_to_cat_loc2";

    [Header("Next Scene")]
    [SerializeField] private string sceneToLoad = "NextLocation";

    private bool waitingForDialogueEnd = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        if (waitingForDialogueEnd)
            return;

        bool hasRequiredFlag = FlagManager.Instance.GetFlag(requiredFlag);

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт, жду окончания...");
            waitingForDialogueEnd = true;
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void Update()
    {
        if (waitingForDialogueEnd && DialogueManager.Instance != null && !DialogueManager.Instance.IsDialogueActive)
        {
            waitingForDialogueEnd = false;
            Debug.Log("Диалог завершён, выполняю переход!");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}