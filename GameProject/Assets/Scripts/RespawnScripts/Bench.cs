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
            PlayerMovement movement = player.GetComponent<PlayerMovement>();

            health.lastBenchPosition = transform.position;

            health.HealFull();

            PlayerPrefs.SetFloat("SavedPosX", transform.position.x);
            PlayerPrefs.SetFloat("SavedPosY", transform.position.y);
            PlayerPrefs.SetInt("HasDash", movement.hasDash ? 1 : 0);
            PlayerPrefs.SetInt("HasDoubleJump", movement.hasDoubleJump ? 1 : 0);
            PlayerPrefs.SetInt("HasWallJump", movement.hasWallJump ? 1 : 0);
            PlayerPrefs.SetInt("HasSaved", 1);
            PlayerPrefs.Save();

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
