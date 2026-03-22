using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public bool isIntroLevelFinish = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isIntroLevelFinish)
            {
                SceneManager.LoadScene("ProceduralLevel");
            }
            else
            {
                GameManager.instance.WinGame();
            }
        }
    }
}
