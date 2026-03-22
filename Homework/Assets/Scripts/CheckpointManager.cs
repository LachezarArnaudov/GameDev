using UnityEngine;
using TMPro;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    [SerializeField] private TextMeshProUGUI checkpointText;
    private int checkpointCollected = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddKey()
    {
        checkpointCollected++;
        checkpointText.text = "Checkpoints: " + checkpointCollected;
    }
}
