using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private bool isGameEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {     
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
