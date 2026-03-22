using UnityEngine;

public class CheckpointFlaged : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Checkpoint up");

            CheckpointManager.instance.AddKey();

            Destroy(gameObject);
        }
    }
}
