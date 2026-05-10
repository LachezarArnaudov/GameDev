using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Speed")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;

    public Transform ledgeCheck;
    public float ledgeDistance = 0.5f;
    public Transform wallCheck;
    public float wallDistance = 0.5f;
    public LayerMask groundLayer;

    [Header("Aggro")]
    public float lineOfSight = 5f;
    public Transform playerTransform;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
   
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distToPlayer < lineOfSight)
        {
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    void Patrol()
    {
        rb.linearVelocity = new Vector2(movingRight ? patrolSpeed : -patrolSpeed, rb.linearVelocity.y);

        bool hasGround = Physics2D.Raycast(ledgeCheck.position, Vector2.down, ledgeDistance, groundLayer);
        bool hitWall = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, wallDistance, groundLayer);

        if (!hasGround || hitWall)
        {
            Flip();
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < playerTransform.position.x && !movingRight)
        {
            Flip();
        }
        else if (transform.position.x > playerTransform.position.x && movingRight)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(movingRight ? chaseSpeed : -chaseSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);

        if (ledgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.down * ledgeDistance);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (movingRight ? Vector3.right : Vector3.left) * wallDistance);
        }
    }
}