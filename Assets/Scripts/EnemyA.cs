using System.Collections;
using UnityEngine;

public class EnemyA : Enemy
{
    private bool hasBeenIlluminated = false; 
    private Coroutine loseInterestCoroutine;

    protected override void FollowPlayerBehavior()
    {
        AudioClip attackSound = Resources.Load<AudioClip>("Sounds/Clips/Dino-roar");
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool flashlightShiningOnEnemy = IsFlashlightShiningOnEnemy();
        
        if (distanceToPlayer <= followRadius && !Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToPlayer, obstacleLayer))
        {
            if (flashlightShiningOnEnemy)
            {
                soundFXManager.Play(attackSound, 0.1f);
                hasBeenIlluminated = true; 
                isPlayerInRange = true;
            }

            if (hasBeenIlluminated)
            {
                MoveWithObstacleAvoidance(playerTransform.position - transform.position);
                FaceTarget(playerTransform.position);

                if (!flashlightShiningOnEnemy)
                {
                    // If the flashlight is no longer shining and the coroutine hasn't started, begin the lose interest delay
                    if (loseInterestCoroutine == null)
                    {
                        loseInterestCoroutine = StartCoroutine(LoseInterestAfterDelay(0.2f));
                    }
                }
                else
                {
                    // If the flashlight is shining on the enemy again, cancel the lose interest delay
                    if (loseInterestCoroutine != null)
                    {
                        StopCoroutine(loseInterestCoroutine);
                        loseInterestCoroutine = null;
                    }
                }
            }
        }
        else if (isPlayerInRange)
        {
            StopFollowingPlayer();
        }

    }

    IEnumerator LoseInterestAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopFollowingPlayer();
        hasBeenIlluminated = false;
    }

    private void StopFollowingPlayer()
    {
        isPlayerInRange = false;
        loseInterestCoroutine = null;
    }

    protected override void ReturnToStartOrPatrol()
    {
    }
}
