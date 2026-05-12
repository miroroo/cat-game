using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour
{
    [Header("Названия сцен")]
    [SerializeField] private string defaultSceneName = "LectureHall"; // Сцена по умолчанию
    [SerializeField] private AudioClip clickSound;

    public void OnClick()
    {
        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        // Проигрываем звук через глобальный менеджер
        if (clickSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(clickSound);
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return null;
        }

        // Загружаем последнюю сцену или сцену по умолчанию
        string sceneToLoad = SceneLoader.Instance != null ? SceneLoader.Instance.GetLastScene() : defaultSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            sceneToLoad = defaultSceneName;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}