using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialButton : MonoBehaviour
{
    [Header("Название сцены обучения")]
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private AudioClip clickSound;

    // Этот метод вызывается при нажатии на кнопку обучения
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

        // Загружаем сцену обучения
        SceneManager.LoadScene(tutorialSceneName);
    }
}