using UnityEngine;

public class Window : InteractableObject
{
    [Header("Messages")]
    [SerializeField] private string Author = "Марсик";
    [SerializeField] private string Message = "Поищи ключ на столе";
    public override void Interact()
    {
        base.Interact();
        DialogueUI.Instance?.Show(
                Author,
                Message,
                DialogueUI.Instance.Hide
            );

    }
}
