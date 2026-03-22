using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private GameObject vignetteOverlay;
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        if (hearts != null && hearts.Length > 0) currentHealth = hearts.Length;
        else currentHealth = 3;
        
        animator = GetComponent<Animator>();
        if (vignetteOverlay != null) vignetteOverlay.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(0.2f, 0.3f);
        }
        UpdateHealthUI();

        if (animator != null)
        {
            animator.SetTrigger("hurt");
        }

        if (vignetteOverlay != null)
        {
            if (currentHealth == 1)
            {
                vignetteOverlay.SetActive(true);
            }           
        }

        if (currentHealth <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
