using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the Boss
        if (collision.gameObject.CompareTag("Boss"))
        {
            // Optionally, you can add a line here to deal damage to the boss if not handled by the boss's script.
            Destroy(gameObject); // Destroy the sprite upon collision
        }
    }
}