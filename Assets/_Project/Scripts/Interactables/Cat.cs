using UnityEngine;

public class Cat : InteractableObject
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Клик по кошке");

        // Первый разговор
        if (!FlagManager.Instance.GetFlag("teacher_table"))
        {
            DialogueManager.Instance.StartDialogue(1);
            return;
        }

        // Ключ ещё не найден
        if (!FlagManager.Instance.GetFlag("door_unlocked"))
        {
            DialogueUI.Instance?.Show(
                "Марсик",
                "Поищи ключ на столе",
                DialogueUI.Instance.Hide
            );
            return;
        }

        // Дверь открыта
        DialogueManager.Instance.StartDialogue(29);
    }
}