using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    public GameObject bossGameObject; // Assign in Inspector
    public GameObject bossHealthBarUI; // Assign in Inspector
    public GameObject SpawnLocation;
    public AudioSource bossMusic; // Assign in Inspector
    public GameObject globalLightGameObject; // Assign the global light GameObject in the inspector

    public SoundFXManager soundFXManager;
    public AudioSource audioSource; 


    private UnityEngine.Rendering.Universal.Light2D globalLight; // For direct manipulation of the Light 2D component

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
                if (globalLightGameObject != null)
    {
        globalLight = globalLightGameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }
    else
    {
        Debug.LogError("Global light GameObject is not assigned in the inspector.");
        return;
    }
            bossGameObject.SetActive(true); // Activate the boss character
            bossHealthBarUI.SetActive(true); // Show the boss's health bar UI
            SpawnLocation.SetActive(true);

             StartCoroutine(FlashLight()); // Start the light flashing coroutine
        
    } 

     private bool stopFlashing = false;

    public void StopFlashingLight()
    {
        stopFlashing = true;
        audioSource.Stop();
        // Optionally, ensure the light has a specific intensity when stopping
        if (globalLight != null)
        {
            globalLight.intensity = 0f; // Adjust to desired intensity
        }
    }

    IEnumerator FlashLight()
    {
        if (globalLight == null) yield break;

        globalLight.color = Color.red;

        while (!stopFlashing)
        {   
            globalLight.color = Color.red;
            globalLight.intensity = 0f; // Light off
            yield return new WaitForSeconds(0.3f);
            globalLight.intensity = 1f; // Light on (adjust intensity as needed)
            yield return new WaitForSeconds(0.3f);
        }

        // Ensure the light has a specific intensity when stopping the flashing
        globalLight.intensity = 0f; // Adjust to default or desired intensity
        globalLight.color = Color.white;
    }

}
