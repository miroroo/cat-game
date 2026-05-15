using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    [Header("Название сцены меню")]
    [SerializeField] private string menuSceneName = "Start";
    [SerializeField] private AudioClip clickSound;

    private bool isLoading = false;
    private static MenuButton existingInstance = null; // Статическая ссылка для проверки дубликатов

    public void OnClick()
    {
        StartCoroutine(LoadSceneWithSound());
    }
    private IEnumerator LoadSceneWithSound()
    {
        // Проигрываем звук через глобальный менеджер
        if (clickSound != null)
        {
            // Ждём появления AudioManager, если его ещё нет
            float waitTime = 0f;
            while (AudioManager.Instance == null && waitTime < 1f)
            {
                yield return null;
                waitTime += Time.deltaTime;
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(clickSound);
                yield return new WaitForSeconds(0.15f); // Небольшая задержка для звука
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null, playing sound via temporary source");
                PlaySoundTemp(clickSound);
                yield return new WaitForSeconds(0.15f);
            }
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

        SceneManager.LoadScene(menuSceneName);
    }

    private void PlaySoundTemp(AudioClip clip)
    {
        if (clip == null) return;

        GameObject tempSound = new GameObject("TempClickSound");
        AudioSource source = tempSound.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        source.Play();
        Destroy(tempSound, clip.length);
    }
}




