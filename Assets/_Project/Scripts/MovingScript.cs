using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        movement.x = 0f;
        movement.y = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            movement.x = -1f;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            movement.x = 1f;

        bool isMoving = movement.x != 0;
        animator.SetBool("isMoving", isMoving);

        if (movement.x > 0)
            sprite.flipX = false;
        else if (movement.x < 0)
            sprite.flipX = true;
    }

    void FixedUpdate()
    {
        if (movement.magnitude < 0.2f)
        {
            movement = Vector2.zero;
        }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}