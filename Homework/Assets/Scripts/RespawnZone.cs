using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = respawnPoint.position;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            HealthManager health = collision.GetComponent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
        }
    }
}
