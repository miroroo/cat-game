using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Вызывается при нажатии на кнопку
    public void OnClick()
    {
        Debug.Log("Выход из игры...");

        // Если игра запущена в редакторе Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Если это собранная игра
        Application.Quit();
#endif
    }
}
