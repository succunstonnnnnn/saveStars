using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject healPlayerPrefab;
    public GameObject healSunPrefab;

    public float spawnRate = 1.5f;
    public float spawnRangeX = 7f;
    public float spawnY = 6f;

    [Range(0f, 1f)] public float healPlayerChance = 0.1f;
    [Range(0f, 1f)] public float healSunChance = 0.1f;

    private bool firstEnemySpawned = false;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            int difficulty = Mathf.FloorToInt(Time.timeSinceLevelLoad / 15f);
            float rate = Mathf.Max(0.3f, spawnRate - difficulty * 0.1f);
            yield return new WaitForSeconds(rate);
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        Vector2 dir = Random.value < 0.5f
            ? Vector2.down
            : new Vector2(Random.value < 0.5f ? -0.5f : 0.5f, -1f);

        if (!firstEnemySpawned)
        {
            firstEnemySpawned = true;
            SpawnEnemy(spawnPos, dir, 1f);
            return;
        }

        int difficulty = Mathf.FloorToInt(Time.timeSinceLevelLoad / 15f);
        float speedMult = 1f + difficulty * 0.1f;
        float currentHealPlayerChance = Mathf.Max(0.02f, healPlayerChance - difficulty * 0.01f);
        float currentHealSunChance    = Mathf.Max(0.02f, healSunChance    - difficulty * 0.01f);

        float roll = Random.value;
        if (roll < currentHealPlayerChance)
            SpawnGoodItem(healPlayerPrefab, spawnPos, dir, HealType.Player);
        else if (roll < currentHealPlayerChance + currentHealSunChance)
            SpawnGoodItem(healSunPrefab, spawnPos, dir, HealType.Sun);
        else
            SpawnEnemy(spawnPos, dir, speedMult);
    }

    void SpawnEnemy(Vector3 pos, Vector2 dir, float speedMult)
    {
        if (enemyPrefab == null) return;
        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.direction = dir;
            ec.speedMultiplier = speedMult;
        }
    }

    void SpawnGoodItem(GameObject prefab, Vector3 pos, Vector2 dir, HealType healType)
    {
        if (prefab == null) return;
        GameObject item = Instantiate(prefab, pos, Quaternion.identity);
        GoodItem gi = item.GetComponent<GoodItem>();
        if (gi != null)
        {
            gi.direction = dir;
            gi.healType = healType;
        }
    }
}
