using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherTable : InteractableObject
{
    [Header("Dialogue")]
    public int requiredId = 17; 

    [Header("Scene")]
    public string sceneToLoad = "FindKey";

    public override void Interact()
    {
        Debug.Log("Нажал на стол");

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

        int currentId = FlagManager.Instance.GetInt("dialogue_progress");

        Debug.Log($"Текущий id диалога = {currentId}");

        if (currentId >= requiredId)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("Сначала завершите диалог");
        }
    }
}