using UnityEngine;

public class AutoWalkCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector3 targetPosition;

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
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (animator != null)
            animator.SetBool(walkParameter, true);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < 0.05f)
        {
            isMoving = false;

            if (animator != null)
                animator.SetBool(walkParameter, false);
        }
    }
}