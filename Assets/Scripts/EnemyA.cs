using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyFollow : MonoBehaviour
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

    private Vector2 startPosition;
    private bool isPlayerInRange = false;
    private bool recentlyIlluminated = false; // New state to track recent illumination
    private Coroutine followDelayCoroutine = null;
    private Coroutine delayedStopCoroutine = null;

    void Start()
    {
        startPosition = transform.position;
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, 0f);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool flashlightShiningOnEnemy = IsFlashlightShiningOnEnemy();

        // If the enemy is within the follow radius and there's no obstacle between them
        if (distanceToPlayer <= followRadius && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            if (flashlightShiningOnEnemy)
            {
                recentlyIlluminated = true;
                isPlayerInRange = true;
                // If illuminated, reset the delayed stop coroutine
                if (delayedStopCoroutine != null)
                {
                    StopCoroutine(delayedStopCoroutine);
                    delayedStopCoroutine = null;
                }
            }
            else if (recentlyIlluminated && delayedStopCoroutine == null)
            {
                // Start delayed stop only after being illuminated once
                delayedStopCoroutine = StartCoroutine(DelayedStopFollowing());
            }
        }

        if (isPlayerInRange)
        {
            FollowPlayerWithObstacleAvoidance();
            FaceTarget();
        }

        EnemyUtilities.AdjustRedOverlay(redOverlay, distanceToPlayer, followRadius);
    }

    IEnumerator DelayedStopFollowing()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        isPlayerInRange = false; // Stop following after delay
        recentlyIlluminated = false; // Reset the recently illuminated state
        delayedStopCoroutine = null;
    }

    private bool IsFlashlightShiningOnEnemy()
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

    void FollowPlayerWithObstacleAvoidance()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        MoveWithObstacleAvoidance(directionToPlayer);
    }

    void MoveWithObstacleAvoidance(Vector2 direction)
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
    }

    void FaceTarget()
    {
        Vector2 directionToTarget = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    void LateUpdate()
    {

        }

    void MoveTowardsStartPositionWithObstacleAvoidance()
    {
        if (!isPlayerInRange)
        {
            Vector2 directionToStart = (startPosition - (Vector2)transform.position).normalized;
            MoveWithObstacleAvoidance(directionToStart);
        }
    }
}
