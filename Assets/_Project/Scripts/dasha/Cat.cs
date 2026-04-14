using UnityEngine;

public class Cat : InteractableObject
{
    public override void Interact()
    {
        base.Interact();

        Debug.Log("Клик по кошке");

        // Запускаем диалог
        DialogueManager.Instance.StartDialogue("cat");

        // СТАВИМ ФЛАГ
        if (FlagManager.Instance != null)
        {
            FlagManager.Instance.SetFlag("talked_to_cat", true);
            Debug.Log("Флаг talked_to_cat установлен");
        }
        else
        {
            Debug.LogError("FlagManager не найден!");
        }
    }
}