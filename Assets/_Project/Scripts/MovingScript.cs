//using UnityEngine;

//public class PlayerClickMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public GameObject LeftWall;

//    private Rigidbody2D rb;
//    private float targetX;
//    private bool isMoving = false;

//    private float minX;
//    private float maxX;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();

//        Camera cam = Camera.main;

//        float height = cam.orthographicSize * 2f;
//        float width = height * cam.aspect;

//        // границы камеры
//        minX = cam.transform.position.x - width / 2f;
//        maxX = cam.transform.position.x + width / 2f;
//    }

//    void Update()
//    {
//        if (Camera.main == null) return;

//        if (Input.GetMouseButtonDown(0))
//        {
//            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//            // Проверяем, есть ли объект под мышкой
//            Collider2D hit = Physics2D.OverlapPoint(mousePos);

//            if (hit != null)
//            {
//                // Если это интерактивный объект — НЕ двигаемся
//                if (hit.GetComponent<InteractableObject>() != null)
//                {
//                    Debug.Log("Клик по объекту, не двигаемся");
//                    return;
//                }
//            }

//            // Иначе — двигаем игрока
//            float mouseX = mousePos.x;

//            targetX = Mathf.Clamp(mouseX, minX, maxX);
//            isMoving = true;
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!isMoving)
//        {
//            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
//            return;
//        }

//        float direction = Mathf.Sign(targetX - transform.position.x);

//        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

//        // Остановка
//        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
//        {
//            isMoving = false;
//            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject == LeftWall)
//        {
//            Debug.Log("Дверь закрыта");
//        }
//    }
//}


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}