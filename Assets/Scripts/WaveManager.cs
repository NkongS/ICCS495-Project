using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public Transform[] spawnPoints;

    public int waveNumber = 1;
    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 3;
    public int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartNewWave();
        }
    }

    void StartNewWave()
    {
        enemiesAlive = enemiesPerWave;
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
        }

        waveNumber++;
        enemiesPerWave += 2;
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.GetComponent<EnemyAI>().player = player;
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            Debug.Log("Wave Complete!");
        }
    }
}
