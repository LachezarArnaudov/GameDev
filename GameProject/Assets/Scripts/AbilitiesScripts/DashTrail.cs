using UnityEngine;

public class DashTrail : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 0.01f;
    private float timer;
    private bool isDashing;

    public void StartTrail(float duration)
    {
        isDashing = true;
        Invoke("StopTrail", duration);
    }

    void StopTrail() { isDashing = false; }

    void Update()
    {
        if (!isDashing) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);

            Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
            ghost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            ghost.transform.localScale = transform.localScale;

            Destroy(ghost, 0.1f);
            timer = spawnInterval;
        }
    }
}
