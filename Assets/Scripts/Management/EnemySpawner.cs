using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Spawn interval for enemies")]
    public float spawnInterval;
    [Tooltip("Minimal spawn interval for game (hardest)")]
    public float minSpawnInterval;
    public int spawnCount;
    public int maxSpawnCount;

    [Header("Difficulty")]
    [Tooltip("In what time interval game gets harder")]
    public float difficultyChangeInterval;
    [Tooltip("How much spawn interval changes per difficulty change")]
    public float spawnIntervalDifficultyChange;
    [Tooltip("How much spawn count changes per difficulty change")]
    public int spawnCountDifficultyChange;

    public int fastEnemySpawnChances, slowEnemySpawnChances;

    public List<Transform> spawns;
    public Transform enemyFolder;

    private void Start()
    {
        StartCoroutine(Spawner());
        StartCoroutine(DifficultyCour());
    }

    private IEnumerator DifficultyCour()
    {
        yield return new WaitForSeconds(difficultyChangeInterval);

        spawnInterval += spawnIntervalDifficultyChange;
        if (spawnInterval <= 0)
            spawnInterval = minSpawnInterval;

        spawnCount += spawnCountDifficultyChange;
        if (spawnCount >= maxSpawnCount) 
            spawnCount = maxSpawnCount;

        StartCoroutine(DifficultyCour());
    }

    private IEnumerator Spawner()
    {
        // Spawn enemies

        if (GameManagerScr.Instance.player.isAlive)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var enemyName = GetRandomEnemyName();
                SpawnEnemy(enemyName, spawns[Random.Range(0, spawns.Count)].position);
            }

            yield return new WaitForSeconds(spawnInterval);

            StartCoroutine(Spawner());
        }
    }

    private string GetRandomEnemyName()
    {
        var enemyName = "Enemy_Normal";

        if (Random.Range(0, 100) < fastEnemySpawnChances)
        {
            enemyName = "Enemy_Fast";
        }
        if (Random.Range(0, 100) < slowEnemySpawnChances)
        {
            enemyName = "Enemy_Slow";
        }

        return enemyName;
    }

    private void SpawnEnemy(string enemyName, Vector3 spawnPos)
    {
        var enemyPref = Resources.Load<GameObject>("Prefabs/Enemies/" + enemyName);
        if (!enemyPref) return;

        var enemyObj = Instantiate(enemyPref, spawnPos, Quaternion.identity);
        enemyObj.transform.SetParent(enemyFolder, true);

        var enemy = enemyObj.GetComponent<Enemy>();
        GameManagerScr.Instance.allEnemies.Add(enemy);
    }
}