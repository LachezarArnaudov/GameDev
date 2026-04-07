using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockUIManager : MonoBehaviour
{
    public static UnlockUIManager Instance;

    public GameObject unlockPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowUnlockScreen(string title, string description)
    {
        titleText.text = title;
        descText.text = description;
        unlockPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (unlockPanel.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            unlockPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
