using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyObject : InteractableObject
{

    [Header("Scene")]
    public string sceneToLoad = "LectureHall";

    public override void Interact()
    {
        Debug.Log("Нажал на ключ");

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        FlagManager.Instance.SetFlag("door_unlocked", true);
        SceneManager.LoadScene(sceneToLoad);

    }
}