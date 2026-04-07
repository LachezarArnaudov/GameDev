using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Checkpoint")]
    public Vector3 lastBenchPosition;

    [Header("UI Hearts")]
    public Image[] hearts;        
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool isInvincible = false;
    public float invincibilityDuration = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        lastBenchPosition = transform.position;
        UpdateHeartsUI();
    }

    public bool TakeDamage(int damage)
    {
        if (isInvincible) return false;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHeartsUI();
        Debug.Log("Player is hit: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        else
        {
            StartCoroutine(BecomeInvincible());
            return false;
        }
    }

    private void UpdateHeartsUI()
    {       
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private System.Collections.IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    public void HealFull()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }

    void Die()
    {
        Debug.Log("Game Over!");
        HealFull();
        isInvincible = false;
        transform.position = lastBenchPosition + new Vector3(0, 1.2f, 0);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
