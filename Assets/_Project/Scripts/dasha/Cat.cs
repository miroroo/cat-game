using UnityEngine;

public class Cat : InteractableObject
{
    public override void Interact()
    {

        Debug.Log("Клик по кошке");

        if (!FlagManager.Instance.GetFlag("talked_to_cat"))
        {
            DialogueManager.Instance.StartDialogue(13);
            FlagManager.Instance.SetFlag("talked_to_cat", true);
        }
        else if (!FlagManager.Instance.GetFlag("door_unlocked"))
        {
            if (DialogueUI.Instance != null)
            {
                DialogueUI.Instance.Show("Марсик", "Поищи ключ на столе", null);
            }
        }
        else if (FlagManager.Instance.GetFlag("door_unlocked"))
        {
            if (DialogueUI.Instance != null)
            {
                DialogueUI.Instance.Show("Марсик", "Я уж думал, что ты не справишься)", null);
            }

        }
    }
}

