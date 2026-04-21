using UnityEngine;

public class AutoWalkCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("Координата X, до которой персонаж должен дойти")]
    [SerializeField] private float targetX = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string walkParameter = "isMoving";

    private bool isMoving = true;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
            return;

        Vector3 currentPosition = transform.position;

        // Двигаемся только по X
        float newX = Mathf.MoveTowards(
            currentPosition.x,
            targetX,
            moveSpeed * Time.deltaTime
        );

        transform.position = new Vector3(
            newX,
            currentPosition.y,
            currentPosition.z
        );

        // Анимация ходьбы
        if (animator != null)
            animator.SetBool(walkParameter, true);

        // Проверка достижения точки
        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
        {
            isMoving = false;

            if (animator != null)
                animator.SetBool(walkParameter, false);

            Debug.Log("Персонаж дошёл до нужной X координаты.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        Gizmos.DrawWireSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}