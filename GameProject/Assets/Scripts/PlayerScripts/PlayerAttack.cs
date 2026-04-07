using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint; 
    public float attackRange = 0.5f; 
    public LayerMask enemyLayers;      
    public int attackDamage = 20;

    [Header("Visual Effects")]
    public GameObject slashEffect;     
    public float slashDuration = 0.1f; 
    public GameObject hitParticlesPrefab;

    [Header("Attack Speed")]
    public float attackRate = 4f;      
    private float nextAttackTime = 0f; 

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
        if (slashEffect != null)
        {
            StartCoroutine(ShowSlashEffect());
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

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
    }

    IEnumerator ShowSlashEffect()
    {
        slashEffect.SetActive(true);
        yield return new WaitForSeconds(slashDuration);
        slashEffect.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
