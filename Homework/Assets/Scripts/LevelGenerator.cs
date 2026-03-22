using System.Collections;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generator settings")]
    [SerializeField] private GameObject[] levelChunks;
    [SerializeField] private GameObject finishLinePrefab;

    [SerializeField] private int numberOfChunks = 10;
    [SerializeField] private float gapBetweenChunks = 3f;
    [SerializeField] private float spawnDelay = 0.3f;

    void Start()
    {
        StartCoroutine(GenerateLevelRoutine());
    }

    private IEnumerator GenerateLevelRoutine()
    {
        float currentX = transform.position.x;

        for (int i = 0; i < numberOfChunks; i++)
        {
            int randomIndex = Random.Range(0, levelChunks.Length);
            GameObject chunkToSpawn = levelChunks[randomIndex];
            GameObject newChunk = Instantiate(chunkToSpawn, new Vector3(currentX, transform.position.y, 0), Quaternion.identity);

            Transform endPoint = newChunk.transform.Find("EndPoint");

            if (endPoint != null)
            {
                currentX = endPoint.position.x + gapBetweenChunks;
            }
            else
            {
                Debug.LogWarning("There is no endPoint in this Chunk: " + newChunk.name);
                currentX += 15f;
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        if (levelChunks.Length > 0)
        {
            Instantiate(levelChunks[0], new Vector3(currentX, transform.position.y, 0), Quaternion.identity);
        }

        if (finishLinePrefab != null)
        {
            Instantiate(finishLinePrefab, new Vector3(currentX, transform.position.y + 0.5f, 0), Quaternion.identity);
        }
    }
}
