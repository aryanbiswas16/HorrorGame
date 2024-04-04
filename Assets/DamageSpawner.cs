using System.Collections;
using UnityEngine;

public class DamageDealerSpawner : MonoBehaviour
{
    public GameObject damageDealerPrefab; // Assign the prefab in the Inspector
    public float spawnAreaWidth = 10f;
    public float spawnAreaHeight = 10f;
    public int spawnAmount = 5; // Total number of objects to spawn
    public float spawnDelay = 1f; // Delay in seconds between spawns

    private void Start()
    {
        StartCoroutine(SpawnDamageDealers());
    }

    IEnumerator SpawnDamageDealers()
    
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Debug.Log("SPAWNED");
            // Generate a random position within the defined area
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
                Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2),
                0) + transform.position; // Adjust Z if necessary for your game's setup

            Instantiate(damageDealerPrefab, spawnPosition, Quaternion.identity, transform);

            // Wait for the specified delay before spawning the next object
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
