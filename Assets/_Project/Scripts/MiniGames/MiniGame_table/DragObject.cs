using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableObject : InteractableObject
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        if (Camera.main == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

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

    void TryStartDrag(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isDragging = true;
            offset = transform.position - (Vector3)mousePos;
        }
    }

    void Drag(Vector2 mousePos)
    {
        transform.position = mousePos + (Vector2)offset;
    }

    void StopDrag()
    {
        isDragging = false;
    }
}