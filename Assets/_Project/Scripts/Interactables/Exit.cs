using UnityEngine;

public class Exit : InteractableObject
{
    [Header("Scene")]
    [SerializeField] private string nextScene = "NextScene";

    [Header("UI")]
    [SerializeField] private GameObject choicePanel;

    private bool isMenuOpen = false;

    public override void Interact()
    {
        base.Interact();

        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
            isMenuOpen = true;

            Time.timeScale = 0f; // пауза игры
        }
    }

    // Кнопка "Выйти"
    public void ExitLocation()
    {
        Time.timeScale = 1f;

        if (SceneLoader.Instance != null)
        {
            DatabaseManager.Instance.ReloadDatabase();
            SceneLoader.Instance.LoadLocation(nextScene);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
    }

    // Кнопка "Искать дальше"
    public void ContinueSearching()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        isMenuOpen = false;
        Time.timeScale = 1f;
    }
}