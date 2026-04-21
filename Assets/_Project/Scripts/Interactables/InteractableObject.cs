using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    private bool isMouseOver = false;
    public string objectName;       // Можно оставить для отладки
    public int itemId;              // ID предмета в базе данных (0, если не предмет)
    //[SerializeField] private GameObject highlightObject;

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

    private void OnMouseEnter()
    {
        //highlightObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        //highlightObject.SetActive(false);
    }

    public virtual void Interact()
    {
        Debug.Log("Взаимодействие с " + objectName);

        // При нажатии на объект воспроизводится звук
        SoundOnClick sound = GetComponent<SoundOnClick>();
        if (sound != null)
        {
            sound.PlaySound();
        }
        
        // Если это предмет (itemId > 0)
        if (itemId > 0)
        {
            Item item = ItemDatabase.GetItem(itemId);
            if (item != null)
            {
                // Проверим, можно ли взять предмет (isTakeable)
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
