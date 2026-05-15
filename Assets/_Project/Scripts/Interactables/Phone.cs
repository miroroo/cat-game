using UnityEngine;

public class Phone : InteractableObject
{
    [Header("Phone UI")]
    [SerializeField] private GameObject phonePanel;
    public GameObject blocker;
    [SerializeField] private Collider2D exitCollider;

    public override void Interact()
    {
        base.Interact();

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт — нельзя взаимодействовать с телефоном");
            return;
        }

        OpenPhone();
    }

    private void OpenPhone()
    {
        phonePanel.SetActive(true);
        blocker.SetActive(true);

        // Просто отключаем коллайдер у Exit
        if (exitCollider != null)
            exitCollider.enabled = false;
    }

    public void ClosePhone()
    {
        phonePanel.SetActive(false);
        blocker.SetActive(false);

        // Включаем коллайдер обратно
        if (exitCollider != null)
            exitCollider.enabled = true;
    }
}