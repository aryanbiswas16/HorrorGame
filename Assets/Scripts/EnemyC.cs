using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyFollowC : MonoBehaviour
{
    public float speed = 5f;
    public float patrolSpeed = 2f;
    public float stoppingDistance = 2f;
    public float followRadius = 10f;
    public float obstacleDetectionDistance = 2f;
    public Transform playerTransform;
    public LayerMask obstacleLayer;
    public Image redOverlay;
    public GameObject flashlight;
    public Transform pointA, pointB; // Patrol points
    public float detectionAngle = 30f;

    private Vector2 startPosition;
    private Vector2 nextPatrolPoint;
    private bool isPlayerInRange = false;
    private Coroutine followDelayCoroutine = null;
    private Coroutine delayedStopCoroutine = null;
    private bool isPatrolling = false;

    void Start()
    {
        startPosition = transform.position;
        nextPatrolPoint = pointA.position;
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, 0f);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool flashlightShiningOnEnemy = IsFlashlightShiningOnEnemy();

        if (distanceToPlayer <= followRadius && flashlightShiningOnEnemy && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            isPlayerInRange = true;
            isPatrolling = false;
            if (delayedStopCoroutine != null)
            {
                StopCoroutine(delayedStopCoroutine);
                delayedStopCoroutine = null;
            }
            FollowPlayerWithObstacleAvoidance();
            FaceTarget(playerTransform.position);
        }
        else if (isPlayerInRange && !flashlightShiningOnEnemy && delayedStopCoroutine == null)
        {
            delayedStopCoroutine = StartCoroutine(DelayedStopFollowing());
        }

        if (!isPlayerInRange && !isPatrolling)
        {
            isPatrolling = true;
            StartCoroutine(Patrol());
        }

        AdjustRedOverlay(distanceToPlayer);
    }

    IEnumerator DelayedStopFollowing()
    {
        yield return new WaitForSeconds(3f);
        isPlayerInRange = false;
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
        FaceTarget((Vector2)transform.position + checkDirection);
    }

    void FaceTarget(Vector2 target)
    {
        Vector2 directionToTarget = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    IEnumerator Patrol()
    {
        while (isPatrolling)
        {
            MoveWithObstacleAvoidance(nextPatrolPoint - (Vector2)transform.position);
            FaceTarget(nextPatrolPoint);

            if (Vector2.Distance(transform.position, nextPatrolPoint) < stoppingDistance)
            {
                nextPatrolPoint = nextPatrolPoint == (Vector2)pointA.position ? pointB.position : pointA.position;
            }
            yield return null;
        }
    }

    public void AdjustRedOverlay(float distanceToPlayer)
    {
        float intensity = 0.8f - Mathf.Clamp01(distanceToPlayer / followRadius);
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, intensity);
    }
}
