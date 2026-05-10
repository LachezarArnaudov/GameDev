using UnityEngine;

public class Bench : MonoBehaviour
{
    private bool canRest = false;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactPrompt;

    void Update()
    {       
        if (canRest && Input.GetKeyDown(interactionKey))
        {
            Rest();
            RespawnEnemies();
        }
    }

    void Rest()
    {
        Debug.Log("Resting...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();

            health.lastBenchPosition = transform.position;

            health.HealFull();

            Debug.Log("Save");
        }
    }

    void RespawnEnemies()
    {
        EnemySpawner[] spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.Spawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canRest = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canRest = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }
}
