using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

// Этот скрипт вешается на GameObject, с которым можно взаимодействовать
public class InteractableObject : MonoBehaviour
{
    private bool isMouseOver = false;
    public string objectName;

    void Update()
    {
        // Рейкаст из позиции мыши
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // Проверяем, попал ли луч в этот объект
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isMouseOver)
            {
                isMouseOver = true;
                OnMouseEnter(); // вызываем наш метод
            }
        }
        else
        {
            if (isMouseOver)
            {
                isMouseOver = false;
                OnMouseExit(); // вызываем наш метод
            }
        }

        // Далее обработка клика (как уже есть)...
        if (Mouse.current.leftButton.wasPressedThisFrame && hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Interact();
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Мышь наведена на " + objectName);
        // Здесь можно, например, изменить цвет спрайта:
        // GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void OnMouseExit()
    {
        Debug.Log("Мышь покинула " + objectName);
        // Вернуть исходный цвет:
        // GetComponent<SpriteRenderer>().color = Color.white;
    }
    public virtual void Interact()
    {
        Debug.Log("Взаимодействие с " + objectName);
    }

}
