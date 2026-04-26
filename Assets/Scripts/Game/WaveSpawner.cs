using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { spawning, waiting, counting };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] enemyPrefab;
        public int count;
        public int points;
        public float rate;
    }
    public Wave[] waves;
    int nextWave = 0;

    public Transform[] spawnPoints;

    [SerializeField] private float timeBetweenWaves = 5;
    private float waveCountdown;

    private float searchCountdown = 1f;

    [SerializeField] private SpawnState state = SpawnState.counting;
    private GameObject prefab;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }
    private void Update()
    {
        if (state == SpawnState.waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waves[nextWave] != null)
        {
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.spawning)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }
    void WaveCompleted()
    {
        Debug.Log("Wave Complete!");
        state = SpawnState.counting;
        waveCountdown = timeBetweenWaves;
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves Complete! Looping...");
        }
        nextWave++;
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);
        state = SpawnState.spawning;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemies(wave.enemyPrefab);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        state = SpawnState.waiting;

        yield break;
    }
    void SpawnEnemies(GameObject[] enemies)
    {
        Debug.Log("Spawning enemies...");
        // Spawn enemies weighted by threat level
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No Spawn points");
            GameObject randE = enemies[Random.Range(0, enemies.Length)];
            prefab = Instantiate(randE, transform.position, transform.rotation);
        }
        else
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject randE = enemies[Random.Range(0, enemies.Length)];
            prefab = Instantiate(randE, spawn.position, spawn.rotation);
        }
    }
}