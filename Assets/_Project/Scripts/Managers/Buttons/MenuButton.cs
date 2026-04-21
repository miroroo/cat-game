using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    [Header("Название сцены меню")]
    [SerializeField] private string menuSceneName = "Start";

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
            Destroy(DialogueUI.Instance.transform.root.gameObject);

        if (DialogueManager.Instance != null)
            Destroy(DialogueManager.Instance.gameObject);

        SceneLoader.Instance.LoadLocation(menuSceneName);
    }
}