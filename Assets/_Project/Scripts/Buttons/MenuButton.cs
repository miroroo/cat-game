using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    [Header("Название сцены меню")]
    [SerializeField] private string menuSceneName = "Start";
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

        // Очистка диалогов
        if (DialogueUI.Instance != null)
            Destroy(DialogueUI.Instance.transform.root.gameObject);

        if (DialogueManager.Instance != null)
            Destroy(DialogueManager.Instance.gameObject);

        // Загрузка сцены
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadLocation(menuSceneName);
        else
            SceneManager.LoadScene(menuSceneName);
    }
}