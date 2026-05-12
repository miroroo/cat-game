using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialButton : MonoBehaviour
{
    [Header("Название сцены обучения")]
    [SerializeField] private string tutorialSceneName = "Tutorial";

    private AudioSource musicSource;
    [SerializeField] private AudioClip clickSound;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.playOnAwake = false;
        musicSource.clip = clickSound;
    }

    // Этот метод вызывается при нажатии на кнопку обучения
    public void OnClick()
    {
        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        if (clickSound != null)
        {
            musicSource.PlayOneShot(clickSound);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return null; // Ждём один кадр, если звука нет
        }

        SceneManager.LoadScene(tutorialSceneName);
    }
}