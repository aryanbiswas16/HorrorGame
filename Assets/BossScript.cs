using UnityEngine;

public class BossCharacter : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject teleportLinePrefab; // Assign in Inspector
    public Transform teleportLineSpawnPoint; // Assign in Inspector
    public BossHealthBarUI healthBarUI;

    public FinishLine FinishLineScript; // Assign this via the Inspector

    public GameObject SpawnLocation;

    public int damageOnCollision = 10; // Damage boss takes on collision
    public float moveSpeed = 3.0f; // Boss movement speed
    public float detectionDistance = 5f; // Distance to detect walls
    private GameObject player; // Reference to the player GameObject

    public DoorController doorController;

    public LayerMask wallLayer; // Assign this in the Inspector

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 lastMoveDirection; // Last movement direction to avoid getting stuck

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player != null)
        {
            FollowPlayer();
            FaceTarget(player.transform.position);
        }
    }

private void OnTriggerEnter2D(Collider2D collider)
{
    if (collider.CompareTag("DamageDealer"))
    {
        Debug.Log("Boss hit by DamageDealer.");
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
        gameObject.SetActive(false);
        doorController.OpenDoor();
        Instantiate(teleportLinePrefab, teleportLineSpawnPoint.position, Quaternion.identity);
        SpawnLocation.SetActive(false);
        healthBarUI.gameObject.SetActive(false);
        FinishLineScript.StopFlashingLight();


    }
    private void FollowPlayer()
    {
        if (player == null) return;

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 checkDirection = directionToPlayer;

        // First, check direct path to player with LayerMask
        if (!IsPathBlocked(checkDirection))
        {
            MoveInDirection(checkDirection);
            return;
        }

        // Try alternative directions with LayerMask
        float angle = 45; // Angle to check for alternative paths
        Vector2 alternativeDirection1 = Quaternion.Euler(0, 0, angle) * directionToPlayer;
        Vector2 alternativeDirection2 = Quaternion.Euler(0, 0, -angle) * directionToPlayer;

        if (!IsPathBlocked(alternativeDirection1))
        {
            MoveInDirection(alternativeDirection1);
        }
        else if (!IsPathBlocked(alternativeDirection2))
        {
            MoveInDirection(alternativeDirection2);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving if all paths are blocked
        }
    }

    private bool IsPathBlocked(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, wallLayer);
        return hit.collider != null;
    }

    private void MoveInDirection(Vector2 direction)
    {
        rb.velocity = direction * moveSpeed;
    }

        protected void FaceTarget(Vector2 target)
    {
        Vector2 directionToTarget = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}