using UnityEngine;

public class Cat : InteractableObject
{
    public override void Interact()
    {
        base.Interact();

        Debug.Log("Клик по кошке");

        DialogueManager.Instance.StartDialogue("student");

    }
}