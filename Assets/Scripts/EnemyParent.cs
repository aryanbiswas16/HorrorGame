using UnityEngine;
using UnityEngine.UI;

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

    protected Vector2 startPosition;
    protected bool isPlayerInRange = false;

    protected virtual void Start()
    {
        startPosition = transform.position;
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, 0);
    }

    protected virtual void Update()
    {
        FollowPlayerBehavior();
        AdjustRedOverlay(Vector2.Distance(transform.position, playerTransform.position));
        if (!isPlayerInRange) ReturnToStartOrPatrol();
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

    protected void AdjustRedOverlay(float distanceToPlayer)
    {
        float intensity = Mathf.Clamp01(1 - distanceToPlayer / followRadius);
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, intensity);
    }

        protected virtual void ReturnToStartOrPatrol()
    {
        MoveWithObstacleAvoidance(startPosition - (Vector2)transform.position);
        FaceTarget(startPosition);
    }
}
