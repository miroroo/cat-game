using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    [Header("Названия сцен")]
    [SerializeField] private string defaultSceneName = "LectureHall"; // Сцена по умолчанию

    public void OnClick()
    {
        // Получаем последнюю сохранённую сцену из FlagManager

        SceneManager.LoadScene(defaultSceneName);
    }

}