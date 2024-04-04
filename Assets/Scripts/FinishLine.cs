using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    public GameObject bossGameObject; // Assign in Inspector
    public GameObject bossHealthBarUI; // Assign in Inspector
    public AudioSource bossMusic; // Assign in Inspector

  private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the Spawn object by its name
            GameObject spawnPoint = GameObject.Find("BossSpawnPoint");

            if (spawnPoint != null)
            {
                // Set the player's position to the Spawn object's position
                other.transform.position = spawnPoint.transform.position;
                // Optionally, add any animations or effects associated with teleportation

                // Activate the boss and its UI components
                ActivateBossScene();
            }
            else
            {
                Debug.LogError("Spawn point not found. Make sure there is a GameObject named 'BossSpawnPoint' in the scene.");
            }
        }
    }

    void ActivateBossScene()
    {
      
            bossGameObject.SetActive(true); // Activate the boss character
            bossHealthBarUI.SetActive(true); // Show the boss's health bar UI
        
    }

}
