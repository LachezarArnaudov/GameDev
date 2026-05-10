using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public float knockbackForce = 5f;

    [Header("Loot")]
    public GameObject coinPrefab;
    public int minCoinsToDrop = 2;
    public int maxCoinsToDrop = 5;

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        currentHealth -= damage;
        Vector2 knockbackDir = (transform.position - (Vector3)damageSourcePosition).normalized;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(knockbackDir.x, 0.2f) * knockbackForce, ForceMode2D.Impulse);
        }

        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " Death!");

        if (coinPrefab != null)
        {
            int coinsToDrop = Random.Range(minCoinsToDrop, maxCoinsToDrop + 1);

            for (int i = 0; i < coinsToDrop; i++)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        this.enabled = false;

        Destroy(gameObject, 0.5f);
    }
}
