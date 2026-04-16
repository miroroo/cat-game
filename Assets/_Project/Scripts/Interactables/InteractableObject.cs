using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    private bool isMouseOver = false;
    public string objectName;       // Можно оставить для отладки
    public int itemId;              // ID предмета в базе данных (0, если не предмет)

    void Update()
    {
        if (Camera.main == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

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

    private void OnMouseEnter()
    {
        //Debug.Log("Мышь наведена на " + objectName);
        // Например, подсветка
        // GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void OnMouseExit()
    {
        //Debug.Log("Мышь покинула " + objectName);
        // GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void Interact()
    {
        Debug.Log("Взаимодействие с " + objectName);

        // Если это предмет (itemId > 0)
        if (itemId > 0)
        {
            Item item = ItemDatabase.GetItem(itemId);
            if (item != null)
            {
                // Проверим, можно ли взять предмет (isTakeable)
                if (item.isTakeable)
                {
                    // Проверим, нет ли уже такого в инвентаре (опционально)
                    // if (!InventoryManager.Instance.HasItem(item.itemName))
                    {
                        InventoryManager.Instance.AddItem(item);
                        // После взятия можно уничтожить объект на сцене или сделать неактивным
                        gameObject.SetActive(false);
                    }
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