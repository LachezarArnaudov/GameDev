using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public int currentCoins = 0;
    public TextMeshProUGUI coinText;

    void Start()
    {
        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            currentCoins = PlayerPrefs.GetInt("CurrentCoins", 0);
        }

        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateUI();
        Debug.Log("Money: " + currentCoins);
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;
        UpdateUI();
        Debug.Log("Spent " + amount + " coins. Remaining: " + currentCoins);
    }

    public void ResetCoins()
    {
        currentCoins = 0;
        UpdateUI();
        Debug.Log("Lost all money bozo!");
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Money: " + currentCoins;
        }
    }
}
