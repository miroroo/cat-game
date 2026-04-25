using UnityEngine;



public class AudioManager : MonoBehaviour
{

    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.5f;

    private static AudioManager Instance;
    private AudioSource musicSource;

    private void Awake()
    {
        // Реализация синглтона
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены

        // Создаём и настраиваем источник для музыки
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = true;

        musicSource.Play();
    }
    
    // Опционально: методы паузы/продолжения
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();
}
