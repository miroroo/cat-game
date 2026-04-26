using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    private string menuSceneName = "Start";

    private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    // Этот метод вызывается при нажатии на кнопку
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
    public void OnClick()
    {
        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        if (backgroundMusic != null)
            musicSource.PlayOneShot(backgroundMusic);

        yield return new WaitForSeconds(0.5f);

        if (DialogueUI.Instance != null)
            DialogueUI.Instance.Hide();

        SceneManager.LoadScene(menuSceneName);
    }
}