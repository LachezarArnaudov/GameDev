using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float aggroRange = 2f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject deathEffect;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < aggroRange)
            {
                ChasePlayer();
            }
            else
            {
                StopChasing();
            }
        }
    }

    private void ChasePlayer()
    {
        float directionX = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(directionX * moveSpeed, rb.linearVelocity.y);
    }

    private void StopChasing()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {          
            Vector2 contactNormal = collision.GetContact(0).normal;

            if (contactNormal.y < -0.5f)
            {               
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {                   
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 4f);
                }

                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
            else
            {
                HealthManager health = collision.gameObject.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                if (animator != null)
                {
                    if (contactNormal.y > 0.5f)
                    {
                        animator.SetTrigger("hitBottom");
                    }
                    else if (contactNormal.x < -0.5f)
                    {
                        animator.SetTrigger("hitRight");
                    }
                    else if (contactNormal.x > 0.5f)
                    {
                        animator.SetTrigger("hitLeft");
                    }
                }
            }
        }
    }
}