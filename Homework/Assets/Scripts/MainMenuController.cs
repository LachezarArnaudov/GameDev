using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OpenOptions()
    {
        print("Open the options menu...");
    }
    public void QuitGame()
    {
        print("Quiting the game!");
        Application.Quit();
    }
}
