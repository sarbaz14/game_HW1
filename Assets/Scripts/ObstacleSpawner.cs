using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public GameObject[] airPrefabs;
    public GameObject coinPrefab;

    public float spawnInterval = 2.2f;
    public float minInterval = 1.5f;
    public float maxInterval = 3.0f;
    public float spawnAheadDistance = 16f;

    public float groundY = -2f;
    public float airY = 0.5f;
    public float coinY = -0.5f;

    private float timer;
    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = spawnInterval;
    }

    private void Update()
    {
        if (PlayerController.Instance == null || PlayerController.Instance.IsDead)
            return;

        // Position spawner ahead of player
        transform.position = new Vector3(PlayerController.Instance.transform.position.x + spawnAheadDistance, transform.position.y, 0f);

        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            timer = 0f;
            nextSpawnTime = Random.Range(minInterval, maxInterval);
            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject()
    {
        // Randomly pick: 0 = Ground Obstacle, 1 = Air Obstacle, 2 = Coin
        int pick = Random.Range(0, 10);

        if (pick < 5 && groundPrefabs != null && groundPrefabs.Length > 0)
        {
            // Ground obstacle (50% chance)
            var prefab = groundPrefabs[Random.Range(0, groundPrefabs.Length)];
            if (prefab != null)
                Instantiate(prefab, new Vector3(transform.position.x, groundY, 0f), Quaternion.identity);
        }
        else if (pick < 8 && airPrefabs != null && airPrefabs.Length > 0)
        {
            // Air obstacle (30% chance)
            var prefab = airPrefabs[Random.Range(0, airPrefabs.Length)];
            if (prefab != null)
                Instantiate(prefab, new Vector3(transform.position.x, airY, 0f), Quaternion.identity);
        }
        else if (coinPrefab != null)
        {
            // Coin (20% chance or bonus)
            Instantiate(coinPrefab, new Vector3(transform.position.x, coinY, 0f), Quaternion.identity);
        }
    }
}
