using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "MainScene";

    public void PlayGame()
    {
        Debug.Log("Loading Game...");

        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Opened options...");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save();

        Debug.Log("RESET!");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}
