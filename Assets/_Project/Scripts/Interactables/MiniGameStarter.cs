using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MiniGameStarter : InteractableObject
{
    [Header("Dialogue Flag")]
    [SerializeField] private string requiredFlag = "teacher_table";

    [Header("Scene")]
    public string sceneToLoad = "FindKey";

    public override void Interact()
    {
        base.Interact();
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
            StartCoroutine(LoadSceneWithSound());
        }
        else
        {
            Debug.Log("Сначала завершите нужный диалог");
        }
    }

    private IEnumerator LoadSceneWithSound()
    {
        yield return new WaitForSeconds(0.5f);

        SceneLoader.Instance.LoadLocation(sceneToLoad);
    }
}