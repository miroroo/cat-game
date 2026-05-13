using UnityEngine;

public class Cat : InteractableObject
{
    [Header("Dialogue Settings")]
    [SerializeField] private int firstMeetingDialogueId = 1;
    [SerializeField] private string flag1 = "teacher_table";
    [SerializeField] private int keyNotFoundDialogueId = 29;
    [SerializeField] private string flag2 = "door_unlocked";
    [SerializeField] private int doorUnlockedDialogueId = 29;

    [Header("Messages")]
    [SerializeField] private string catName = "Марсик";
    [SerializeField] private string keyHintMessage = "Поищи ключ на столе";

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Клик по кошке");

        DialogueUI.Instance.gameObject.SetActive(true);

        // Первый разговор
        if (!FlagManager.Instance.GetFlag(flag1))
        {
            DialogueManager.Instance.StartDialogue(firstMeetingDialogueId);
            return;
        }

        // Ключ ещё не найден
        if (!FlagManager.Instance.GetFlag(flag2))
        {
            DialogueUI.Instance?.Show(
                catName,
                keyHintMessage,
                DialogueUI.Instance.Hide
            );
            return;
        }

        // Дверь открыта
        DialogueManager.Instance.StartDialogue(doorUnlockedDialogueId);
    }
}