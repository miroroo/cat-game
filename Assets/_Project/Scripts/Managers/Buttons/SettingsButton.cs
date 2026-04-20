using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour
{
    [Header("Название сцены меню")]
    [SerializeField] private string menuSceneName = "Settings";

    // Этот метод вызывается при нажатии на кнопку
    public void OnClick()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}