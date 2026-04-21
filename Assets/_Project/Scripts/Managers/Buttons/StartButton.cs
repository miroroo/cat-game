using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour
{
    [Header("Названия сцен")]
    [SerializeField] private string defaultSceneName = "LectureHall"; // Сцена по умолчанию

    private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    private void Start()
    {
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
        musicSource.PlayOneShot(backgroundMusic);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneLoader.Instance.GetLastScene());
    }

}