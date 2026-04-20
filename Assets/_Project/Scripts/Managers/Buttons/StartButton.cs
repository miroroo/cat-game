using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    [Header("Названия сцен")]
    [SerializeField] private string defaultSceneName = "LectureHall"; // Сцена по умолчанию

    public void OnClick()
    {
        // Получаем последнюю сохранённую сцену из FlagManager
        //bool lastScene = GetLastScene();

        //// Загружаем нужную сцену
        //if (!string.IsNullOrEmpty(lastScene))
        //{
        //    Debug.Log($"Загружаем последнюю сцену: {lastScene}");
        //    SceneManager.LoadScene(lastScene);
        //}
        //else
        //{
        //    Debug.Log($"Нет сохранений. Загружаем сцену по умолчанию: {defaultSceneName}");
        //    SceneManager.LoadScene(defaultSceneName);
        //}

        SceneManager.LoadScene(defaultSceneName);
    }

    //private string GetLastScene()
    //{
    //    // Вариант 1: Если FlagManager статический
    //    if (FlagManager.Instance != null)
    //    {
    //        string lastScene = FlagManager.Instance.GetFlag("LastScene");
    //        if (!lastScene)
    //            return lastScene;
    //    }

    //    return false; 
    //}
}