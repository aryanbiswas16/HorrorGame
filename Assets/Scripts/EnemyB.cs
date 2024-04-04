using System.Collections;
using UnityEngine;

public class EnemyB : Enemy
{
    private Coroutine delayedStopCoroutine = null;
    private bool recentlyIlluminated = false;

    protected override void FollowPlayerBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool flashlightShiningOnEnemy = IsFlashlightShiningOnEnemy();
        AudioClip attackSound = Resources.Load<AudioClip>("Sounds/Clips/Zombie-growl");

        if (distanceToPlayer <= followRadius && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            if (flashlightShiningOnEnemy)
            {
                
                recentlyIlluminated = true;
                isPlayerInRange = true;
                // Reset the delayed stop coroutine
                if (delayedStopCoroutine != null)
                {
                    soundFXManager.Play(attackSound, 0.1f);
                    StopCoroutine(delayedStopCoroutine);
                    delayedStopCoroutine = null;
                }
                MoveWithObstacleAvoidance(playerTransform.position - transform.position);
                FaceTarget(playerTransform.position);
            }
            else if (recentlyIlluminated && delayedStopCoroutine == null)
            {
                delayedStopCoroutine = StartCoroutine(DelayedStopFollowing());
            }
        }

    }

    IEnumerator DelayedStopFollowing()
    {
        yield return new WaitForSeconds(3f);
        isPlayerInRange = false;
        recentlyIlluminated = false;
        delayedStopCoroutine = null;
    }

        protected override void ReturnToStartOrPatrol()
    {
        base.ReturnToStartOrPatrol(); // Optionally, customize for EnemyB
    }
}
