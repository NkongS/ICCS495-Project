using UnityEngine;
using System.Collections;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject carPrefab;
    public GameObject logPrefab;
    public GameObject levelSegmentPrefab;
    public GameObject endingPlatformPrefab;
    public Transform player;
    public Transform[] enemySpawnPoints;
    public Transform[] carSpawnPoints;
    public Transform[] logSpawnPoints;

    public int waveNumber = 1;
    public float timeBetweenWaves = 30f;
    public int enemiesPerWave = 20;
    public int enemiesAlive = 0;
    public int levelSegments = 5;

    private bool waveInProgress = false;

    void Start()
    {
        GenerateLevel();
        StartCoroutine(StartFirstWave());
        StartCoroutine(SpawnCars());
        StartCoroutine(SpawnLogs());
    }

    void GenerateLevel()
    {
        Renderer renderer = levelSegmentPrefab.GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("No Renderer found in levelSegmentPrefab or its children.");
            return;
        }

        float segmentLength = renderer.bounds.size.z; 
        float offset = 10f; 
        Vector3 startPosition = Vector3.zero;

        enemySpawnPoints = new Transform[0];
        carSpawnPoints = new Transform[0];
        logSpawnPoints = new Transform[0];

        for (int i = 0; i < levelSegments; i++)
        {
            Vector3 position = startPosition + new Vector3(0, 0, i * (segmentLength + offset)); 
            GameObject segment = Instantiate(levelSegmentPrefab, position, Quaternion.identity);

            Transform bridge = segment.transform.Find("Bridge");
            if (bridge != null)
            {
                float randomX = Random.Range(-60f, 60f);
                Vector3 bridgePosition = bridge.localPosition;
                bridge.localPosition = new Vector3(randomX, bridgePosition.y, bridgePosition.z);
            }

            enemySpawnPoints = CombineArrays(enemySpawnPoints, segment.GetComponentsInChildren<Transform>().Where(t => t.CompareTag("EnemySpawnPoint")).ToArray());
            carSpawnPoints = CombineArrays(carSpawnPoints, segment.GetComponentsInChildren<Transform>().Where(t => t.CompareTag("CarSpawnPoint")).ToArray());
            logSpawnPoints = CombineArrays(logSpawnPoints, segment.GetComponentsInChildren<Transform>().Where(t => t.CompareTag("LogSpawnPoint")).ToArray());
        }

        Vector3 endingPosition = startPosition + new Vector3(-18.5f, 3.2f, levelSegments * segmentLength + offset + 4); 
        Instantiate(endingPlatformPrefab, endingPosition, Quaternion.identity);
    }

    Transform[] CombineArrays(Transform[] array1, Transform[] array2)
    {
        Transform[] combinedArray = new Transform[array1.Length + array2.Length];
        array1.CopyTo(combinedArray, 0);
        array2.CopyTo(combinedArray, array1.Length);
        return combinedArray;
    }

    IEnumerator StartFirstWave()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(StartNewWave());
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (waveNumber <= 5)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            if (!waveInProgress)
            {
                StartCoroutine(StartNewWave());
            }
        }
    }

    IEnumerator StartNewWave()
    {
        waveInProgress = true;
        enemiesAlive = enemiesPerWave;
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.25f);
        }

        waveNumber++;
        waveInProgress = false;
    }

    void SpawnEnemy()
    {
        if (enemySpawnPoints.Length == 0) return;
        Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.GetComponent<EnemyAI>().player = player;
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            SpawnCar();
        }
    }

    void SpawnCar()
    {
        if (carSpawnPoints.Length == 0) return;
        Transform spawnPoint = carSpawnPoints[Random.Range(0, carSpawnPoints.Length)];
        Instantiate(carPrefab, spawnPoint.position, carPrefab.transform.rotation);
    }

    IEnumerator SpawnLogs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            SpawnLog();
        }
    }

    void SpawnLog()
    {
        if (logSpawnPoints.Length == 0) return;
        Transform spawnPoint = logSpawnPoints[Random.Range(0, logSpawnPoints.Length)];
        Instantiate(logPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            Debug.Log("Wave Complete!");
            waveInProgress = false;
        }
    }
}