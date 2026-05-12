using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool isMouseOver = false;
    public string objectName;
    public int itemId;
    [SerializeField] private AudioClip interactionSound;

    void Update()
    {
        if (Camera.main == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("interactables");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, mask);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isMouseOver)
            {
                isMouseOver = true;
                OnMouseEnter();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Interact();
            }
        }
        else
        {
            if (isMouseOver)
            {
                isMouseOver = false;
                OnMouseExit();
            }
        }
    }

    private void Start()
    {
        // Не нужно создавать AudioSource, звуки идут через AudioManager
    }

    private void OnMouseEnter() { }
    private void OnMouseExit() { }

    public virtual void Interact()
    {
        Debug.Log("Взаимодействие с " + objectName);

        // Проигрываем звук через глобальный менеджер
        if (interactionSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(interactionSound);
        }

        if (itemId > 0)
        {
            Item item = ItemDatabase.GetItem(itemId);
            if (item != null)
            {
                if (item.isTakeable)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Этот предмет нельзя взять");
                }
            }
            else
            {
                Debug.LogError($"Предмет с ID {itemId} не найден в базе");
            }
        }
    }
}