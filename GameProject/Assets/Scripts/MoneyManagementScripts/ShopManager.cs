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
        PlayerCurrency wallet = FindFirstObjectByType<PlayerCurrency>();
        PlayerHealth health = FindFirstObjectByType<PlayerHealth>();

        if (wallet.currentCoins >= healthUpgradePrice && health.maxHealth < maxHeartsLimit)
        {
            wallet.currentCoins -= healthUpgradePrice;
            wallet.AddCoins(0);

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
        PlayerCurrency wallet = FindFirstObjectByType<PlayerCurrency>();
        PlayerAttack attack = FindFirstObjectByType<PlayerAttack>();

        if (attack != null && wallet.currentCoins >= damageUpgradePrice)
        {
            wallet.currentCoins -= damageUpgradePrice;
            wallet.AddCoins(0);
            attack.attackDamage += 25;
            Debug.Log("Succesful attack increase. Current attack damage is: " + attack.attackDamage);
        } 
    }
    public void BuyHealPotion()
    {
        PlayerCurrency wallet = FindFirstObjectByType<PlayerCurrency>();
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        if (wallet.currentCoins >= healPotionPrice)
        {
            wallet.currentCoins -= healPotionPrice;
            wallet.AddCoins(0);

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
