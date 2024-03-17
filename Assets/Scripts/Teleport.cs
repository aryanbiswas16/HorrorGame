using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
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
                // Optionally, add any animations or effects associated with teleportation
            }
            else
            {
                Debug.LogError("Spawn point not found. Make sure there is a GameObject named 'Spawn' in the scene.");
            }
        }
    }
}
