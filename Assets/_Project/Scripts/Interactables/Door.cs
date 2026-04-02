using UnityEngine;

public class Door : InteractableObject
{
    public string doorFlagId = "door_loc1"; // ID флага в FlagManager

    public override void Interact()
    {
        // Сначала вызываем базовый метод (для отладки)
        base.Interact();

        // Проверяем флаг двери
        bool flagValue = FlagManager.Instance.GetFlag(doorFlagId);

        if (flagValue == true)
        {
            Debug.Log("дверь открыта");
            // Здесь можно добавить анимацию открытия двери
            // Например: GetComponent<Animator>().SetTrigger("Open");
        }
        else
        {
            Debug.Log("дверь закрыта");
            // Здесь можно добавить подсказку, что дверь заперта
        }
    }
}