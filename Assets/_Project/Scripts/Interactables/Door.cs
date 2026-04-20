using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneToLoad = "Coridor";


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт — нельзя взаимодействовать");
            return;
        }

        bool isUnlocked = FlagManager.Instance.GetFlag("door_unlocked");

        if (isUnlocked)
        {
            DialogueUI.Instance.Message("", "Дверь открыта...",null);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            DialogueUI.Instance.Message("","Дверь закрыта, возможно, поможет ключ?", null);
        }
    }
}