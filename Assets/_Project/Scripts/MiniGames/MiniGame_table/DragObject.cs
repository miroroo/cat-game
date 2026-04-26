using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс для перетаскиваемого объекта.
/// Позволяет захватывать объект мышью и перемещать его по сцене.
/// </summary>
public class DraggableObject : InteractableObject
{
    // Флаг активного перетаскивания
    private bool isDragging = false;

    // Смещение между курсором и позицией объекта
    private Vector3 offset;

    /// <summary>
    /// Отслеживает нажатие, удержание и отпускание кнопки мыши
    /// для управления перетаскиванием объекта.
    /// </summary>
    void Update()
    {
        if (Camera.main == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        // НАЧАЛО
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryStartDrag(mousePos);
        }

        // ПРОЦЕСС
        if (isDragging)
        {
            Drag(mousePos);
        }

        // КОНЕЦ
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopDrag();
        }
    }

    /// <summary>
    /// Проверяет, был ли нажат текущий объект,
    /// и запускает перетаскивание.
    /// </summary>
    void TryStartDrag(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isDragging = true;
            offset = transform.position - (Vector3)mousePos;
        }
    }

    /// <summary>
    /// Перемещает объект за курсором мыши
    /// с учётом начального смещения.
    /// </summary>
    void Drag(Vector2 mousePos)
    {
        transform.position = mousePos + (Vector2)offset;
    }

    /// <summary>
    /// Завершает перетаскивание объекта.
    /// </summary>
    void StopDrag()
    {
        isDragging = false;
    }
}