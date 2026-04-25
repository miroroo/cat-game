using UnityEngine;

public class AutoWalkCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;   // Скорость движения
    [SerializeField] private float targetX;          // Целевая X-координата

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string walkParameter = "isMoving";

    private bool isMoving = true;
    private float startY; // Запоминаем начальную Y-позицию

    private void Start()
    {
        // Если Animator не назначен вручную — ищем на объекте
        if (animator == null)
            animator = GetComponent<Animator>();

        // Запоминаем стартовую Y, чтобы герой не двигался по вертикали
        startY = transform.position.y;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        // Текущее положение
        float currentX = transform.position.x;

        // Двигаем только по X
        float newX = Mathf.MoveTowards(
            currentX,
            targetX,
            moveSpeed * Time.deltaTime
        );

        transform.position = new Vector3(
            newX,
            startY,
            transform.position.z
        );

        // Включаем анимацию ходьбы
        if (animator != null)
            animator.SetBool(walkParameter, true);

        // Проверяем, дошёл ли герой
        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
        {
            isMoving = false;

            // Выключаем анимацию
            if (animator != null)
                animator.SetBool(walkParameter, false);
        }
    }

    // Визуализация точки назначения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        float y = Application.isPlaying ? startY : transform.position.y;

        Vector3 targetPosition = new Vector3(
            targetX,
            y,
            transform.position.z
        );

        Gizmos.DrawWireSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}