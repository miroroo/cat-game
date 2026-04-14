using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherTable : InteractableObject
{
    public string catDialogueFlagId = "talked_to_cat";

    public override void Interact()
    {
        Debug.Log("Нажал на стол");

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        bool flagValue = FlagManager.Instance.GetFlag(catDialogueFlagId);
        Debug.Log("Флаг = " + flagValue);

        if (flagValue)
        {
            Debug.Log("Загружаю сцену FindKey");
            SceneManager.LoadScene("FindKey");
        }
        else
        {
            Debug.Log("Сначала поговорите с кошкой");
        }
    }
}