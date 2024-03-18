using System.Collections;
using UnityEngine;

public class EnemyC : Enemy
{
    public Transform pointA, pointB;
    private Vector2 nextPatrolPoint;
    private Coroutine patrolCoroutine;
    private Coroutine delayBeforePatrolCoroutine; // New coroutine variable for handling delay

    protected override void Start()
    {
        base.Start();
        nextPatrolPoint = pointA.position;
        // Start patrolling immediately without delay.
        patrolCoroutine = StartCoroutine(Patrol());
    }

    protected override void FollowPlayerBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool flashlightShiningOnEnemy = IsFlashlightShiningOnEnemy();

        if (distanceToPlayer <= followRadius && flashlightShiningOnEnemy && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            // When starting to chase the player, stop any existing patrol or delay.
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
                patrolCoroutine = null;
            }
            if (delayBeforePatrolCoroutine != null)
            {
                StopCoroutine(delayBeforePatrolCoroutine);
                delayBeforePatrolCoroutine = null;
            }
            isPlayerInRange = true;
            MoveWithObstacleAvoidance(playerTransform.position - transform.position);
            FaceTarget(playerTransform.position);
        }
        else if (isPlayerInRange || patrolCoroutine == null && delayBeforePatrolCoroutine == null)
        {
            isPlayerInRange = false;
            if (delayBeforePatrolCoroutine == null)
            {
                delayBeforePatrolCoroutine = StartCoroutine(DelayBeforePatrol(2f)); 
            }
        }
    }

    IEnumerator DelayBeforePatrol(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isPlayerInRange)
        {
            patrolCoroutine = StartCoroutine(Patrol());
        }
        delayBeforePatrolCoroutine = null; // Reset the delay handle after the delay has passed and patrol started.
    }

    IEnumerator Patrol()
    {
        while (true) // Infinite loop for patrolling
        {
            if (Vector2.Distance(transform.position, nextPatrolPoint) < stoppingDistance)
            {
                // Switch the patrol point
                nextPatrolPoint = nextPatrolPoint == (Vector2)pointA.position ? pointB.position : pointA.position;
                // Add a slight pause at each patrol point for realism.
                yield return new WaitForSeconds(1f);
            }
            MoveWithObstacleAvoidance(nextPatrolPoint - (Vector2)transform.position);
            FaceTarget(nextPatrolPoint);
            yield return null;
        }
    }

    protected override void ReturnToStartOrPatrol()
    {
    }
}
