using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public float knockbackForce = 5f;

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

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        GetComponent<Collider2D>().enabled = false;

        this.enabled = false;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        Destroy(gameObject, 0.5f); 
    }
}
