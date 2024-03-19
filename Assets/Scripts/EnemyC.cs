using System.Collections;
using UnityEngine;

public class EnemyC : Enemy
{
    public Transform pointA, pointB;
    private Vector2 nextPatrolPoint;
    private Coroutine patrolCoroutine;
    private Coroutine delayBeforePatrolCoroutine;
    private enum State { Patrolling, Chasing, Waiting }
    private State currentState;

    protected override void Start()
    {
        base.Start();
        ChangeState(State.Patrolling);
        nextPatrolPoint = pointB.position;

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FollowPlayerBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool flashlightShiningOnEnemy = IsFlashlightShiningOnEnemy();

        if (distanceToPlayer <= followRadius && flashlightShiningOnEnemy && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            if (currentState != State.Chasing)
            {
                ChangeState(State.Chasing);
            }
            MoveWithObstacleAvoidance(playerTransform.position - transform.position);
            FaceTarget(playerTransform.position);
        }
        else if (currentState == State.Chasing)
        {
            ChangeState(State.Waiting);
        }

    }

    private void ChangeState(State newState)
    {
        

        switch (currentState)
        {
            case State.Patrolling:
                if (patrolCoroutine != null)
                {
                    StopCoroutine(patrolCoroutine);
                }
                break;
            case State.Waiting:
                if (delayBeforePatrolCoroutine != null)
                {
                    StopCoroutine(delayBeforePatrolCoroutine);
                }
                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case State.Patrolling:
                patrolCoroutine = StartCoroutine(Patrol());
                break;
            case State.Waiting:
                delayBeforePatrolCoroutine = StartCoroutine(DelayBeforePatrol(2f));
                break;
        }
    }

    IEnumerator DelayBeforePatrol(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentState == State.Waiting)
        {
            ChangeState(State.Patrolling);
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, nextPatrolPoint) < stoppingDistance)
            {
                nextPatrolPoint = nextPatrolPoint == (Vector2)pointA.position ? pointB.position : pointA.position;
                yield return new WaitForSeconds(1f); // Wait time at each patrol point
            }
            else
            {
                MoveWithObstacleAvoidance(nextPatrolPoint - (Vector2)transform.position);
                FaceTarget(nextPatrolPoint);
                yield return null;
            }
        }
    }

    protected override void ReturnToStartOrPatrol()
    {
        // This might be called if specific logic requires the enemy to return to a start point or resume patrolling.
        if (!isPlayerInRange && currentState != State.Patrolling)
        {
            ChangeState(State.Patrolling);
        }
    }
}
