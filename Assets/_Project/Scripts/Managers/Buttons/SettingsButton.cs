using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsButton : MonoBehaviour
{
    [Header("Название сцены меню")]
    [SerializeField] private string menuSceneName = "Settings";

    private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;
    private void Start()
    {
        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.playOnAwake = false;
        musicSource.clip = backgroundMusic;
    }

    // Этот метод вызывается при нажатии на кнопку
    public void OnClick()
    {
        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        musicSource.PlayOneShot(backgroundMusic);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(menuSceneName);
    }
}