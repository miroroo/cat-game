using UnityEngine;

public class ButterBread : InteractableObject
{

    public override void Interact()
    {
        base.Interact();
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог уже идёт");
            return;
        }

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден");
            return;
        }

        DialogueUI.Instance.Show(
                "Марсик",
                "О... бутерброд. Скромное, но достойное подношение для такого великого кота, как я.",
                null);
    }
}