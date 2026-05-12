using UnityEngine;

public class Phone : InteractableObject
{
    [Header("Phone UI")]
    [SerializeField] private GameObject phonePanel;

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
        if (phonePanel == null)
        {
            Debug.LogError("PhonePanel не назначен в Inspector!");
            return;
        }

        phonePanel.SetActive(true);
        Debug.Log("Телефон открыт крупным планом");
    }

    public void ClosePhone()
    {
        if (phonePanel != null)
            phonePanel.SetActive(false);
    }
}