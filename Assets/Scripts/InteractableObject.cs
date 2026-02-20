using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Этот скрипт вешается на GameObject, с которым можно взаимодействовать
public class InteractableObject : MonoBehaviour
{
    public string objectName; // имя объекта (для диалогов)

    public virtual void Interact()
    {
        Debug.Log("Взаимодействие с " + objectName);
    }

    private void OnMouseEnter()
    {
        // показываем текст
    }

    private void OnMouseExit()
    {
        // Возвращаем как было
    }
}