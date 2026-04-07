using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int spikeDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            PlayerMovement movement = collision.GetComponent<PlayerMovement>();

            if (health != null)
            {
                bool hasDied = health.TakeDamage(spikeDamage);

                if (!hasDied && movement != null)
                {
                    collision.transform.position = movement.lastSafePosition + new Vector3(0, 0.5f, 0);
                    collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
            }
        }
    }
}
