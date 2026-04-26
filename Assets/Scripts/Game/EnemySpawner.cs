using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float minSpawnTime, maxSpawnTime;
    private float timeUntilSpawn;
    private void Awake()
    {
        SetTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            int randE = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[randE], transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }
    void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }
}