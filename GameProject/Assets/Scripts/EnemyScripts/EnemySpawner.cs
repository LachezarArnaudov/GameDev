using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy that spawns here")]
    public GameObject enemyPrefab;

    private GameObject currentEnemy;

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }

        if (enemyPrefab != null)
        {
            currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
