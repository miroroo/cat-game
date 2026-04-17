using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneToLoad = "TestScene";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        bool isUnlocked = FlagManager.Instance.GetFlag("door_unlocked");

        if (isUnlocked)
        {
            Debug.Log("Дверь открыта — переходим");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("Дверь закрыта");
        }
    }
}