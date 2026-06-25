using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public int healPotions = 0;

    [Header("UI Elements")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI potionCountText;

    private bool isInventoryOpen = false;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            healPotions = PlayerPrefs.GetInt("HealPotions", 0);
        }

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        Time.timeScale = isInventoryOpen ? 0f : 1f;
        UpdateUI();
    }

    public void AddPotion(int amount)
    {
        healPotions += amount;
        UpdateUI();
    }

    public void UsePotion()
    {
        if (healPotions > 0 && playerHealth.currentHealth < playerHealth.maxHealth)
        {
            healPotions--;
            playerHealth.HealFull();
            UpdateUI();
            Debug.Log("Heal");
        }
        else if (playerHealth.currentHealth >= playerHealth.maxHealth)
        {
            Debug.Log("Max health already");
        }
    }

    void UpdateUI()
    {
        if (potionCountText != null)
        {
            potionCountText.text = "Potions: " + healPotions;
        }
    }
}
