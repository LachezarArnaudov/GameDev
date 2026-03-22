using UnityEngine;

public class ApplePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Good job boss, you got the apple!");

            Destroy(gameObject);
        }
    }
}
