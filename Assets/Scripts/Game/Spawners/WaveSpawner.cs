using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance { get; private set; }
    public enum SpawnState { spawning, waiting, counting };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public List<WeightedSpawnSO> WeightedEnemies = new List<WeightedSpawnSO>();
        public int count;
        public int points;
        public float rate;
    }
    public Wave[] waves;
    [SerializeField] private float[] Weights;
    public int nextWave = 0;

    private int numPoints = 0;
    public Transform[] spawnPoints;
    private GameObject arenaObject;
    private Arena arena;

    public GameObject[] enemyPrefab;
    private EnemySO enemySO;
    public Dictionary<string, EnemySO> enemyList;

    [SerializeField] private float timeBetweenWaves = 5;
    private float waveCountdown;

    private float searchCountdown = 1f;

    [SerializeField] private SpawnState state = SpawnState.counting;
    private GameObject prefab;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        Weights = new float[waves[nextWave].WeightedEnemies.Count];
    }

    private void Start()
    {
        arenaObject = GameObject.FindGameObjectWithTag("Arena");
        arena = arenaObject.GetComponent<Arena>();
        numPoints = arena.GetSpawnCount();
        if (spawnPoints == null) { spawnPoints = new Transform[numPoints]; }
        //for (int i = 0; i < spawnPoints.Length; i++)
        //{
        //    spawnPoints[i] = ArenaManager.Instance.spawnPoints[i];
        //}
        //SetEnemySpawns();
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
        DifficultyController.Instance.wavesCompleted++;
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
        //ResetSpawnWeights();

        // Difficulty Scaling
        //float num = wave.count * DifficultyController.Instance.difficulty;
        //wave.count = Mathf.RoundToInt(num);

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemies(wave.WeightedEnemies);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        state = SpawnState.waiting;

        yield break;
    }
    void SpawnEnemies(List<WeightedSpawnSO> enemies)
    {
        Debug.Log("Spawning enemies...");
        // Spawn enemies weighted by threat level
        for (int j = 0; j < DifficultyController.Instance.difficulty; j++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            float value = Random.value;
            for (int i = 0; i < Weights.Length; i++)
            {
                if (value < Weights[i])
                {
                    //  FIX
                    if (spawn != null)
                    {
                        prefab = Instantiate(waves[nextWave].WeightedEnemies[i].enemy, spawn.position, spawn.rotation);
                    }
                }
                value -= Weights[i];
            }
            ResetSpawnWeights();
        }
    }
    public void ManualSpawn(string id)
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        foreach (var obj in enemyPrefab)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy == null) { Debug.Log(id + " is null!"); return; }
            if (id.Equals(enemy.enemy.enemyID))
            {
                prefab = Instantiate(obj, spawn.position, spawn.rotation);
            }
        }
    }
    void ResetSpawnWeights()
    {
        float totalWeight = 0;
        for (int i = 0; i < waves[nextWave].WeightedEnemies.Count; i++)
        {
            Weights[i] = waves[nextWave].WeightedEnemies[i].GetWeight();
            totalWeight += Weights[i];
        }
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] /= totalWeight;
        }
    }
    public void SetEnemySpawns()
    {
        arenaObject = GameObject.FindGameObjectWithTag("Arena");
        arena = arenaObject.GetComponent<Arena>();
        spawnPoints = arena.GetEnemySpawns();
    }
}