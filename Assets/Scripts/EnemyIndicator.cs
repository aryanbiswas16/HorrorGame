using UnityEngine;
using System.Collections.Generic;

public class EnemyIndicatorSystem : MonoBehaviour
{
    public GameObject indicatorPrefab;
    private Transform playerTransform;
    private float detectionRadius = 5f;
    private List<GameObject> indicators = new List<GameObject>();
    private Dictionary<Enemy, GameObject> enemyToIndicator = new Dictionary<Enemy, GameObject>();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InitializePool();
    }

    void Update()
    {
        UpdateIndicators();
    }

    void InitializePool()
    {
        // Adjust pool size as needed
        int poolSize = 10;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject indicator = Instantiate(indicatorPrefab, transform);
            indicator.SetActive(false);
            indicators.Add(indicator);
        }
    }

    GameObject GetIndicatorFromPool()
    {
        foreach (GameObject indicator in indicators)
        {
            if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
                return indicator;
            }
        }
        // Optionally expand the pool if all indicators are in use
        return null;
    }

    void UpdateIndicators()
    {
        // Optionally, clear mappings if enemies are destroyed or no longer valid
        ClearInvalidMappings();

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        HashSet<Enemy> detectedEnemies = new HashSet<Enemy>();

        foreach (var enemy in enemies)
        {
            Vector2 directionToEnemy = enemy.transform.position - playerTransform.position;
            if (directionToEnemy.magnitude <= detectionRadius)
            {
                detectedEnemies.Add(enemy);
                GameObject indicator = enemyToIndicator.ContainsKey(enemy) ? enemyToIndicator[enemy] : GetIndicatorFromPool();
                if (indicator != null)
                {
                    UpdateIndicatorPositionAndRotation(indicator, directionToEnemy.normalized);
                    enemyToIndicator[enemy] = indicator;
                }
            }
        }

        // Deactivate indicators for enemies that are no longer detected
        foreach (var pair in new Dictionary<Enemy, GameObject>(enemyToIndicator))
        {
            if (!detectedEnemies.Contains(pair.Key))
            {
                pair.Value.SetActive(false);
                enemyToIndicator.Remove(pair.Key);
            }
        }
    }

void UpdateIndicatorPositionAndRotation(GameObject indicator, Vector2 directionToEnemy)
{
    // Assuming directionToEnemy is normalized
    float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
    indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

    // Example for placing the indicator on the bottom edge of the screen
    // You'll need to adjust this logic based on actual game requirements
    indicator.transform.position = new Vector3(Screen.width / 2, 50, 0);
}
    void ClearInvalidMappings()
    {
        var toRemove = new List<Enemy>();
        foreach (var pair in enemyToIndicator)
        {
            if (pair.Key == null || !pair.Key.gameObject.activeInHierarchy)
            {
                pair.Value.SetActive(false);
                toRemove.Add(pair.Key);
            }
        }
        foreach (var key in toRemove)
        {
            enemyToIndicator.Remove(key);
        }
    }
}
