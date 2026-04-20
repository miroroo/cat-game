using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [Header("Название сцены меню")]
    [SerializeField] private string menuSceneName = "Start";

    // Этот метод вызывается при нажатии на кнопку
    public void OnClick()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}