using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsButton : MonoBehaviour
{
    [Header("Название сцены")]
    [SerializeField] private string settingsSceneName = "Settings";
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

        // Загружаем сцену настроек
        SceneManager.LoadScene(settingsSceneName);
    }
}