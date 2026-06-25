using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject shopPanel;
    public GameObject interactPrompt;

    [Header("Prices")]
    public int healthUpgradePrice = 15;
    public int damageUpgradePrice = 20;
    public int healPotionPrice = 5;

    [Header("Limits")]
    public int maxHeartsLimit = 7;

    private bool isPlayerNear = false;
    private bool isShopOpen = false;

    private PlayerCurrency wallet;
    private PlayerHealth health;
    private PlayerAttack attack;
    private PlayerInventory inventory;

    void Start()
    {
        wallet = FindFirstObjectByType<PlayerCurrency>();
        health = FindFirstObjectByType<PlayerHealth>();
        attack = FindFirstObjectByType<PlayerAttack>();
        inventory = FindFirstObjectByType<PlayerInventory>();

        if (wallet == null) Debug.LogWarning("ShopManager: PlayerCurrency not found!");
        if (health == null) Debug.LogWarning("ShopManager: PlayerHealth not found!");
        if (attack == null) Debug.LogWarning("ShopManager: PlayerAttack not found!");
        if (inventory == null) Debug.LogWarning("ShopManager: PlayerInventory not found!");
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.SetActive(isShopOpen);

        if (isShopOpen)
        {
            Time.timeScale = 0f;
            interactPrompt.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void BuyHealthUpgrade()
    {
        if (wallet == null || health == null) return;

        if (wallet.currentCoins >= healthUpgradePrice && health.maxHealth < maxHeartsLimit)
        {
            wallet.SpendCoins(healthUpgradePrice);
            health.maxHealth += 1;
            health.HealFull();
            Debug.Log("Succesful health increase");
        }
        else if (health.maxHealth >= maxHeartsLimit)
        {
            Debug.Log("Can't buy more hearts! You already have the max!!");
        }
    }

    public void BuyDamageUpgrade()
    {
        if (wallet == null || attack == null) return;

        if (wallet.currentCoins >= damageUpgradePrice)
        {
            wallet.SpendCoins(damageUpgradePrice);
            attack.attackDamage += 25;
            Debug.Log("Succesful attack increase. Current attack damage is: " + attack.attackDamage);
        }
    }
    public void BuyHealPotion()
    {
        if (wallet == null || inventory == null) return;

        if (wallet.currentCoins >= healPotionPrice)
        {
            wallet.SpendCoins(healPotionPrice);
            inventory.AddPotion(1);
            Debug.Log("Bought a potion.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactPrompt.SetActive(false);

            if (isShopOpen) ToggleShop();
        }
    }
}