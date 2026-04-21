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
        musicSource.PlayOneShot(backgroundMusic);
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт — нельзя взаимодействовать");
            return;
        }

        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(menuSceneName);
    }
}