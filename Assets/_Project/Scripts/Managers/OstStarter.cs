using UnityEngine;

public class OstStarter : MonoBehaviour
{
    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    public static OstStarter Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Передаём музыку в AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusic(backgroundMusic, musicVolume);
        }
    }
}