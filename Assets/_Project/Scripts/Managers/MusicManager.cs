using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    // Singleton-экземпляр аудиоменеджера
    private static MusicManager Instance;

    // Источник воспроизведения фоновой музыки
    private AudioSource musicSource;

    /// <summary>
    /// Создаёт Singleton-экземпляр, настраивает AudioSource
    /// и запускает фоновую музыку.
    /// </summary>
    private void Awake()
    {
        // Реализация синглтона
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Создаём и настраиваем источник для музыки
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = true;

        musicSource.Play();
    }

    /// <summary>
    /// Ставит воспроизведение музыки на паузу.
    /// </summary>
    public void PauseMusic() => musicSource.Pause();

    /// <summary>
    /// Продолжает воспроизведение музыки после паузы.
    /// </summary>
    public void ResumeMusic() => musicSource.UnPause();
}