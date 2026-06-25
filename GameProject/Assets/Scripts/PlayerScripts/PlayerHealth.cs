using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Checkpoint")]
    public Vector3 lastBenchPosition;

    [Header("UI Hearts")]
    public GameObject heartPrefab;
    public Transform heartsParent;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    [Header("Audio Settings")]
    public AudioClip hurtSound;
    private AudioSource audioSource;
    private PlayerMovement playerMovement;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        lastBenchPosition = transform.position;
        UpdateHeartsUI();
    }

    public bool TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        if (isInvincible) return false;

        audioSource.PlayOneShot(hurtSound);

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHeartsUI();

        playerMovement.ApplyKnockback(damageSourcePosition);

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

    public void UpdateHeartsUI()
    {
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsParent);

            if (i < currentHealth)
            {
                newHeart.GetComponent<Image>().sprite = fullHeart;
            }
            else
            {
                newHeart.GetComponent<Image>().sprite = emptyHeart;
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
        PlayerCurrency wallet = GetComponent<PlayerCurrency>();
        if (wallet != null)
        {
            wallet.ResetCoins();
        }

        HealFull();
        isInvincible = false;
        transform.position = lastBenchPosition + new Vector3(0, 1.2f, 0);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}