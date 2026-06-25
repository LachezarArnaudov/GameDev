using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPointSide;
    public Transform attackPointUp;
    public Transform attackPointDown;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    [Header("Visual Effects")]
    public GameObject slashEffect;
    public float slashDuration = 1f;
    public GameObject hitParticlesPrefab;

    [Header("Attack Speed")]
    public float attackRate = 4f;
    private float nextAttackTime = 0f;

    [Header("Pogo Mechanic (Отскок)")]
    public float pogoBounceForce = 12f;
    public LayerMask pogoLayers;       
    private Rigidbody2D rb;

    [Header("Audio Settings")]
    public AudioClip attackSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Transform currentAttackPoint = attackPointSide;
        float verticalInput = Input.GetAxisRaw("Vertical");
        float zRotation = 0f;
        bool isAttackingDown = false; 

        if (verticalInput > 0)
        {
            currentAttackPoint = attackPointUp;
            zRotation = 90f;
        }
        else if (verticalInput < 0)
        {
            currentAttackPoint = attackPointDown;
            zRotation = -90f;
            isAttackingDown = true; 
        }

        if (slashEffect != null)
        {
            slashEffect.transform.position = currentAttackPoint.position;
            slashEffect.transform.localRotation = Quaternion.Euler(0, 0, zRotation);
            StartCoroutine(ShowSlashEffect());
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(currentAttackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemyScript = enemyCollider.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage, transform.position);
                if (hitParticlesPrefab != null)
                {
                    Instantiate(hitParticlesPrefab, enemyCollider.transform.position, Quaternion.identity);
                }
            }
        }

        if (isAttackingDown)
        {
            Collider2D[] pogoHits = Physics2D.OverlapCircleAll(currentAttackPoint.position, attackRange, pogoLayers);

            if (pogoHits.Length > 0)
            {
                PogoBounce();
            }
        }
    }

    void PogoBounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 

        rb.AddForce(Vector2.up * pogoBounceForce, ForceMode2D.Impulse);

        Debug.Log("POGO Отскок!");
    }

    IEnumerator ShowSlashEffect()
    {
        slashEffect.SetActive(true);
        yield return new WaitForSeconds(slashDuration);
        slashEffect.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackPointSide != null) Gizmos.DrawWireSphere(attackPointSide.position, attackRange);
        if (attackPointUp != null) Gizmos.DrawWireSphere(attackPointUp.position, attackRange);
        if (attackPointDown != null) Gizmos.DrawWireSphere(attackPointDown.position, attackRange);
    }
}