using UnityEngine;

public class BossCharacter : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject teleportLinePrefab; // Assign in Inspector
    public Transform teleportLineSpawnPoint; // Assign in Inspector
    public BossHealthBarUI healthBarUI;
    public int damageOnCollision = 10; // Damage boss takes on collision

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider has the tag "DamageDealer"
        if (collision.gameObject.CompareTag("DamageDealer"))
        {
            TakeDamage(damageOnCollision);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBarUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Disable the boss character
        gameObject.SetActive(false);
        // Instantiate the teleport line
        Instantiate(teleportLinePrefab, teleportLineSpawnPoint.position, Quaternion.identity);
    }
}
