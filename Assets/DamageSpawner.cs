using System.Collections;
using UnityEngine;

public class DamageDealerSpawner : MonoBehaviour
{
    public GameObject damageDealerPrefab; // Assign the prefab in the Inspector
    public float spawnAreaWidth = 10f;
    public float spawnAreaHeight = 10f;
    public int spawnAmount = 15; // Total number of objects to spawn
    public float spawnDelay = 1f; // Delay in seconds between spawns

    private GameObject lastSpawned; // To keep track of the last spawned object

    private void Start()
    {
        StartCoroutine(SpawnDamageDealers());
    }

    IEnumerator SpawnDamageDealers()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            // Wait for the last spawned object to be destroyed
            while (lastSpawned != null)
            {
                yield return null; // Wait until the next frame before checking again
            }

            Debug.Log("SPAWNED");
            // Generate a random position within the defined area
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
                Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2),
                0) + transform.position;

            // Instantiate the new object and keep a reference to it
            lastSpawned = Instantiate(damageDealerPrefab, spawnPosition, Quaternion.identity, transform);

            // Wait for the specified delay before allowing the next spawn
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
