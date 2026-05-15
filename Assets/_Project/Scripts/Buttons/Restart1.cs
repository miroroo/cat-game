using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void OnClick()
    {

        StartCoroutine(LoadSceneWithSound());
    }

    private IEnumerator LoadSceneWithSound()
    {
        if (clickSound != null && AudioManager.Instance != null)
        {
            Debug.Log("Проигрываем звук клика");
            AudioManager.Instance.PlaySound(clickSound);
            yield return new WaitForSeconds(0.3f);
        }

        DatabaseManager.Instance.ReloadDatabase();
    }
}