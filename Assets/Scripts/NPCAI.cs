using System.Collections;
using UnityEngine;
using Mirror;

public class NPCAI : NetworkBehaviour
{
    public string NameOfAI = "default";
    public Vector2 TargetPosition;
    [SerializeField]
    private float jumpForce = 5f; // Force for jumping
    [SerializeField]
    public float walkingSpeed = 2f; // Horizontal walking speed

    [SerializeField]
    private float acceleration = 2f; // Чим більше значення, тим швидший розгін
    [SerializeField]
    private LayerMask obstacleLayer; // Layer mask for obstacles
    [SerializeField]
    private float jumpCooldown = 1f; // Cooldown duration for jumping

    public Rigidbody2D rb;
    public Transform thisTransform;
    private bool isWalking = false;
    public bool TargetIsEnemy;

    private bool canJump = true; // Flag to control jump cooldown

    void Start()
    {
        if (!isServer) return;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (thisTransform == null)
        {
            thisTransform = transform;
        }
    }

    void FixedUpdate()
    {
        if (isWalking)
        {
            Navigate();
        }
    }

    public void WalkNow(Vector2 targetPosition, bool targetIsEnemy)
    {
        TargetPosition = targetPosition;
        isWalking = true;
        TargetIsEnemy = targetIsEnemy;
    }

    public void StopWalking()
    {
        isWalking = false;
        TargetPosition = Vector2.zero;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Navigate()
    {
        // Визначаємо напрямок руху
        Vector2 direction = (TargetPosition - (Vector2)thisTransform.position).normalized;

        // Поточна горизонтальна швидкість
        float currentSpeed = rb.velocity.x;

        // Визначаємо бажану швидкість з урахуванням максимального ліміту
        float targetSpeed = direction.x * walkingSpeed;

        // Плавне збільшення швидкості
        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        // Оновлюємо швидкість об'єкта
        rb.velocity = new Vector2(newSpeed, rb.velocity.y);

        // Перевірка на потребу стрибка
        if (TargetPosition.y - thisTransform.position.y > 2 && IsGrounded())
        {
            TryJump();
        }
        else if (IsObstacleInFront() && IsGrounded())
        {
            TryJump();
        }

        // Зупинка, якщо близько до цілі
        if (Vector2.Distance(thisTransform.position, TargetPosition) < 0.1f && !TargetIsEnemy)
        {
            StopWalking();
        }
    }


    private void TryJump()
    {
        if (canJump)
        {
            Jump();
            StartCoroutine(JumpCooldownRoutine());
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private IEnumerator JumpCooldownRoutine()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private bool IsGrounded()
    {
        // Check if the NPC is on the ground using a small raycast downwards
        RaycastHit2D hit = Physics2D.Raycast(thisTransform.position, Vector2.down, 0.7f, obstacleLayer);
        return hit.collider != null;
    }

    private bool IsObstacleInFront()
    {
        // Check if there is an obstacle in front of the NPC using a small raycast
        Vector2 direction = new Vector2(thisTransform.localScale.x, 0); // Left or right
        RaycastHit2D hit = Physics2D.Raycast(thisTransform.position, direction, 0.7f, obstacleLayer);
        return hit.collider != null;
    }
}
