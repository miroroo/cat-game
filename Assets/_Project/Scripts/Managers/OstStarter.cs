using UnityEngine;

public class OstStarter : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager not found!");
            return;
        }

        if (backgroundMusic != null)
        {
            AudioManager.Instance.SetMusic(backgroundMusic, musicVolume);
        }
    }
}