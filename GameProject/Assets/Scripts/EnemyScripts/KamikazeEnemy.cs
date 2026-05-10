using UnityEngine;
using System.Collections; 

public class KamikazeEnemy : MonoBehaviour
{
    [Header("Settings")]
    public float diveSpeed = 8f;        
    public float detectionRadius = 6f;
    public float chargeTime = 1.2f;  
    public int damage = 1;

    [Header("Loot")]
    public GameObject coinPrefab;
    public int minCoinsToDrop = 1;
    public int maxCoinsToDrop = 2;

    private Transform player;
    private bool isCharging = false;
    private bool isDiving = false;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || isCharging || isDiving) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < detectionRadius && player.position.y < transform.position.y)
        {
            StartCoroutine(ChargeAndDive());
        }
    }

    IEnumerator ChargeAndDive()
    {
        isCharging = true;

        float timer = 0;
        Vector3 originalPos = transform.position;

        while (timer < chargeTime)
        {
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.1f;

            spriteRenderer.color = Color.Lerp(Color.white, Color.red, timer / chargeTime);

            timer += Time.deltaTime;
            yield return null;
        }

        targetPosition = player.position;
        isCharging = false;
        isDiving = true;
        spriteRenderer.color = Color.red;
    }

    void FixedUpdate()
    {
        if (isDiving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, diveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDiving || isCharging)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
                if (health != null && !health.isInvincible)
                {
                    health.TakeDamage(damage, transform.position);
                }
            }
            Explode();
        }
    }

    void Explode()
    {
        if (coinPrefab != null)
        {
            int coinsToDrop = Random.Range(minCoinsToDrop, maxCoinsToDrop + 1);
            for (int i = 0; i < coinsToDrop; i++)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}