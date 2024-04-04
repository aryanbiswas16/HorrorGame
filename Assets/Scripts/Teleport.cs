using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements like Panel

public class Teleport : MonoBehaviour
{
    public ActivateChaseScript chaseScript; // Assign this via the Inspector
    public GameObject transitionPanel; // Assign a UI Panel via the Inspector
    public AudioSource teleportSound; // Assign an AudioSource with your teleport sound clip via the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the Spawn object by its name
            GameObject spawnPoint = GameObject.Find("Level2SpawnPoint");
            
            if (spawnPoint != null)
            {
                // Set the player's position to the Spawn object's position
                other.transform.position = spawnPoint.transform.position;
                chaseScript.StopFlashingLight();
                
                StartCoroutine(TeleportRoutine());
            }
            else
            {
                Debug.LogError("Spawn point not found. Make sure there is a GameObject named 'Level2SpawnPoint' in the scene.");
            }
        }
    }

    IEnumerator TeleportRoutine()
    {
        // Play the teleport sound
        teleportSound.Play();

        // Enable the transition panel to make it visible
        transitionPanel.SetActive(true);

        // Wait for 3 seconds
        yield return new WaitForSeconds(3);

        // Disable the transition panel to hide it
        transitionPanel.SetActive(false);
    }
}
