using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour
{
    [Header("Названия сцен")]
    [SerializeField] private string defaultSceneName = "LectureHall";
    [SerializeField] private AudioClip clickSound;

    public void OnClick()
    {

        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        if (clickSound != null && AudioManager.Instance != null)
        {
            Debug.Log("Проигрываем звук клика");
            AudioManager.Instance.PlaySound(clickSound);
            yield return new WaitForSeconds(0.3f);
        }

        string sceneToLoad = SceneLoader.Instance != null ? SceneLoader.Instance.GetLastScene() : defaultSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            sceneToLoad = defaultSceneName;
        }


        // Проверяем доступность сцены
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }
}