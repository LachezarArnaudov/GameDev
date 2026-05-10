using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-3f, 3f);
            float randomY = Random.Range(3f, 6f);
            rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCurrency wallet = collision.gameObject.GetComponent<PlayerCurrency>();
            if (wallet != null)
            {
                wallet.AddCoins(coinValue);
                Destroy(gameObject);
            }
        }
    }
}
