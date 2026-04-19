using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherTable : InteractableObject
{
    [Header("Dialogue Flag")]
    [SerializeField] private string requiredFlag = "talked_to_cat_loc1";

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

        bool isDialogueCompleted =
            FlagManager.Instance.GetFlag(requiredFlag);

        Debug.Log(
            $"Флаг {requiredFlag} = {isDialogueCompleted}"
        );

        if (isDialogueCompleted)
        {
            Debug.Log($"Загружаем сцену: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("Сначала завершите нужный диалог");
        }
    }
}