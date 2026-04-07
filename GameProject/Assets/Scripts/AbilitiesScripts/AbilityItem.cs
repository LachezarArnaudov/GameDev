using UnityEngine;

public class AbilityItem : MonoBehaviour
{
    public bool unlockDash;
    public bool unlockDoubleJump;

    [Header("UI Message")]
    public string popupTitle = "New Ability";
    public string popupDescription = "Press SHIFT to perform a dash";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();

            if (unlockDash) pm.hasDash = true;
            if (unlockDoubleJump) pm.hasDoubleJump = true;

            UnlockUIManager.Instance.ShowUnlockScreen(popupTitle, popupDescription);

            Destroy(gameObject);
        }
    }
}
