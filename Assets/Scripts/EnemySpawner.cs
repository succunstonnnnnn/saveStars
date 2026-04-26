using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int totalEnemies = 10;
    public float spawnRate = 1.5f;
    public float spawnRangeX = 7f;
    public float spawnY = 6f;

    private int spawnedCount = 0;
    private float nextSpawnTime = 0f;

    void Update()
    {
        if (spawnedCount >= totalEnemies) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
            spawnedCount++;

            if (spawnedCount >= totalEnemies)
            {
                Debug.Log("Хвилю завершено! Всі астрофаги випущені.");
            }
        }
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}