using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTests
{
    [UnityTest]
    public IEnumerator PlayerLosesHealthWhenHit()
    {
        GameObject player = new GameObject("TestPlayer");
        HealthManager health = player.AddComponent<HealthManager>();

        yield return null;

        int initialHealth = health.GetCurrentHealth();
        Assert.AreEqual(3, initialHealth);

        health.TakeDamage(1);

        Assert.AreEqual(2, health.GetCurrentHealth());

        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator PlayerChangesYPositionWhenJumpingOnSpring()
    {
        GameObject player = new GameObject("TestPlayer");
        player.tag = "Player";
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();

        player.transform.position = new Vector3(0, 0, 0);

        GameObject springObject = new GameObject("TestSpring");
        JumpPad spring = springObject.AddComponent<JumpPad>();

        yield return null;

        float initialY = player.transform.position.y;

        rb.linearVelocity = new Vector2(0, spring.bounceForce);

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(player.transform.position.y > initialY);

        Object.Destroy(player);
        Object.Destroy(springObject);
    }
}
