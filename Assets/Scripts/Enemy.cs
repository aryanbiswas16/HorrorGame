using UnityEngine;
using System.Collections;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 5f;
    public float stoppingDistance = 2f;
    public float followRadius = 10f;
    public float obstacleDetectionDistance = 2f;
    public Transform playerTransform;
    public LayerMask obstacleLayer;

    private Vector2 startPosition;
    private bool isPlayerInRange = false;
    private Coroutine followDelayCoroutine;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Check if the player is within the follow radius and there are no obstacles in between
        if (distanceToPlayer <= followRadius && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            isPlayerInRange = true;
            FollowPlayerWithObstacleAvoidance();
        }
        else
        {
            // If the player is out of range or obstructed, stop following and eventually return to start if not already doing so
            if (followDelayCoroutine == null)
            {
                followDelayCoroutine = StartCoroutine(FollowDelay());
            }
        }
    }

    IEnumerator FollowDelay()
    {
        yield return new WaitForSeconds(5f);
        isPlayerInRange = false;
        // Ensure coroutine is set to null so it can be restarted if the player re-enters the follow radius
        followDelayCoroutine = null;
    }

    void MoveTowardsStartPositionWithObstacleAvoidance()
    {
        if (!isPlayerInRange)
        {
            // Re-use the obstacle avoidance logic from following, but target the start position
            Vector2 directionToStart = (startPosition - (Vector2)transform.position).normalized;
            MoveWithObstacleAvoidance(directionToStart);
        }
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
            // Attempt to find a clear path
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

    void LateUpdate()
    {
        // Call the method to return to start position in LateUpdate to avoid conflict with follow logic
        MoveTowardsStartPositionWithObstacleAvoidance();
    }
}
