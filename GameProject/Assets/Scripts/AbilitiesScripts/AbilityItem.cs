using UnityEngine;

public class AbilityItem : MonoBehaviour
{
    public bool unlockDash;
    public bool unlockDoubleJump;
    public bool unlockWallJump; 

    [Header("UI Message")]
    public string popupTitle = "New Ability";
    public string popupDescription = "Press SHIFT to perform a dash";

    void Start()
    {
        if (unlockDash && PlayerPrefs.GetInt("HasDash", 0) == 1)
        {
            Destroy(gameObject);
        }
        else if (unlockDoubleJump && PlayerPrefs.GetInt("HasDoubleJump", 0) == 1)
        {
            Destroy(gameObject);
        }
        else if (unlockWallJump && PlayerPrefs.GetInt("HasWallJump", 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();

            if (unlockDash) pm.hasDash = true;
            if (unlockDoubleJump) pm.hasDoubleJump = true;
            if (unlockWallJump) pm.hasWallJump = true; 

            UnlockUIManager.Instance.ShowUnlockScreen(popupTitle, popupDescription);

            Destroy(gameObject);
        }
    }
}