using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public abstract class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public float stoppingDistance = 2f;
    public float followRadius = 10f;
    public float obstacleDetectionDistance = 2f;
    public Transform playerTransform;
    public LayerMask obstacleLayer;
    public Image redOverlay;
    public GameObject flashlight;
    public float detectionAngle = 30f;
    public SoundFXManager soundFXManager;


    protected Vector2 startPosition;
    protected bool isPlayerInRange = false;

    public GameObject enemyIndicatorPrefab;
    private Dictionary<GameObject, GameObject> enemyIndicators = new Dictionary<GameObject, GameObject>();

    public float indicatorSpawnRadius = 10f;

    protected virtual void Start()
    {
        startPosition = transform.position;
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, 0);
        soundFXManager = SoundFXManager.GetInstance();
    }

    protected virtual void Update()
    {
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        isPlayerInRange = distanceToPlayer <= followRadius; // Or any other logic defining 'in range'
        FollowPlayerBehavior();
        AdjustRedOverlay(2f+ distanceToPlayer);
        if (!isPlayerInRange) ReturnToStartOrPatrol();
        UpdateEnemyIndicators();
        DuplicateIndicatorForNearbyEnemies();
    }

    protected abstract void FollowPlayerBehavior();

    protected bool IsFlashlightShiningOnEnemy()
    {
        Flashlight flashlightScript = flashlight.GetComponent<Flashlight>();
        if (flashlightScript != null && flashlightScript.isFlashlightOn)
        {
            Vector2 directionToEnemy = transform.position - flashlight.transform.position;
            float angleBetween = Vector2.Angle(flashlight.transform.right, directionToEnemy);

            return angleBetween <= detectionAngle / 2f;
        }
        return false;
    }

    protected void MoveWithObstacleAvoidance(Vector2 direction)
    {
        Vector2 checkDirection = direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayer);

        if (hit.collider != null)
        {
            checkDirection = Quaternion.Euler(0, 0, 45) * direction;
            hit = Physics2D.Raycast(transform.position, checkDirection, obstacleDetectionDistance, obstacleLayer);
            if (hit.collider != null)
            {
                checkDirection = Quaternion.Euler(0, 0, -90) * direction;
            }
        }

        Vector2 targetPosition = Vector2.MoveTowards(transform.position, (Vector2)transform.position + checkDirection, speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(targetPosition);
        FaceTarget((Vector2)transform.position + checkDirection);
    }

    protected void FaceTarget(Vector2 target)
    {
        Vector2 directionToTarget = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

/// <summary>
/// 
/// </summary>
/// 

    protected void UpdateEnemyIndicators()
    {
        // Assuming you want one indicator per enemy...
        AdjustIndicatorVisibilityAndRotation();

        // If your game needs to dynamically create and destroy indicators based on proximity...
        DuplicateIndicatorForNearbyEnemies();
    }

    private void AdjustIndicatorVisibilityAndRotation()
    {
        // Example of rotating a single indicator towards the player
        // This can be expanded or modified depending on your specific requirements
        if (enemyIndicatorPrefab != null && isPlayerInRange)
        {
            Vector2 directionToPlayer = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f)); // Adjusting for sprite orientation
            enemyIndicatorPrefab.transform.rotation = rotation;

            // Ensure the indicator is enabled or visible
            if (!enemyIndicatorPrefab.activeSelf)
            {
                enemyIndicatorPrefab.SetActive(true);
            }
        }
        else if (enemyIndicatorPrefab != null)
        {
            // Hide the indicator if the player is not in range
            enemyIndicatorPrefab.SetActive(false);
        }
    }

        private void DuplicateIndicatorForNearbyEnemies()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, indicatorSpawnRadius, 1 << LayerMask.NameToLayer("Ignore Raycast"));
        HashSet<GameObject> currentEnemies = new HashSet<GameObject>();

        // Check each nearby enemy
        foreach (var enemyCollider in nearbyEnemies)
        {
            GameObject enemy = enemyCollider.gameObject;
            // Skip self
            if (enemy == gameObject) continue;

            currentEnemies.Add(enemy);

            // If this enemy doesn't have an indicator yet, create one
            if (!enemyIndicators.ContainsKey(enemy))
            {
                GameObject newIndicator = Instantiate(enemyIndicatorPrefab, transform.position, Quaternion.identity);
                enemyIndicators[enemy] = newIndicator;
            }

            // Update indicator position and rotation
            UpdateIndicatorPositionAndRotation(enemy, enemyIndicators[enemy]);
        }

        // Remove indicators for enemies that are no longer nearby
        List<GameObject> enemiesToRemove = new List<GameObject>();
        foreach (var enemy in enemyIndicators.Keys)
        {
            if (!currentEnemies.Contains(enemy))
            {
                Destroy(enemyIndicators[enemy]);
                enemiesToRemove.Add(enemy);
            }
        }

        foreach (var enemy in enemiesToRemove)
        {
            enemyIndicators.Remove(enemy);
        }
    }

    private void UpdateIndicatorPositionAndRotation(GameObject enemy, GameObject indicator)
    {
        Vector3 directionToEnemy = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
        indicator.transform.position = transform.position + directionToEnemy.normalized; // Or some other logic to place the indicator
        indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust based on indicator's orientation
    }

    private List<GameObject> FindAllEnemies()
{
    List<GameObject> allEnemies = new List<GameObject>();
    foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
    {
        if (enemy != this.gameObject) // Exclude self
        {
            allEnemies.Add(enemy);
        }
    }
    return allEnemies;
}


///

    protected void AdjustRedOverlay(float distanceToPlayer)
    {
        float intensity = 0.8f - Mathf.Clamp01(distanceToPlayer / followRadius);
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, intensity);
    }

        protected virtual void ReturnToStartOrPatrol()
    {
        MoveWithObstacleAvoidance(startPosition - (Vector2)transform.position);
        FaceTarget(startPosition);
    }
}
