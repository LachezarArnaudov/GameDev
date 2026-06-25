using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Debug.Log("MainMenu back");

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
