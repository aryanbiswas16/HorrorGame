using UnityEngine;

public class DamageDealerSpawner : MonoBehaviour
{
    public GameObject damageDealerPrefab; // Assign the prefab in the Inspector
    public float spawnAreaWidth = 10f;
    public float spawnAreaHeight = 10f;
    public int spawnAmount = 5; // Number of objects to spawn

    void Start()
    {
        SpawnDamageDealers();
    }

    public void SpawnDamageDealers()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            // Generate a random position within the defined area
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
                Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2),
                0) + transform.position; // Adjust Z if necessary for your game's setup

            Instantiate(damageDealerPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }
}
