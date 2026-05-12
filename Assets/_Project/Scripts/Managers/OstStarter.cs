using UnityEngine;

public class OstStarter : MonoBehaviour
{
    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    public static OstStarter Instance { get; private set; }
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = true;
        musicSource.tag = "Music"; // Добавляем тег Music

        musicSource.Play();
    }

    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();

    // Метод для установки громкости из настроек
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    // Метод для получения текущей громкости
    public float GetMusicVolume() => musicSource != null ? musicSource.volume : 0f;
}