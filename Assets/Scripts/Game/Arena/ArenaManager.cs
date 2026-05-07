using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance { get; private set; }
    public GameObject[] arenaPool;
    public List<GameObject> oldArenas = new List<GameObject>();
    private GameObject prefab;
    private GameObject arenaObject;
    public Arena arena;

    public Transform[] spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        arenaObject = GameObject.FindGameObjectWithTag("Arena");
        if (arenaObject == null) { NewArena(); }
        arena = arenaObject.GetComponent<Arena>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Transform SpawnLocation(int index)
    {
        return spawnPoints[index];
    }
    private void SetSpawnLocations()
    {
        for (int i = 0; i < arena.GetSpawnCount(); i++)
        {
            spawnPoints[i].transform.position = arena.GetEnemySpawns()[i].transform.position;
        }
    }
    public void NewArena()
    {
        ArchiveArena(arenaObject);
        prefab = Instantiate(arenaPool[Random.Range(0, arenaPool.Length)], transform);
        arenaObject = prefab;
    }
    void ArchiveArena(GameObject arena)
    {
        oldArenas.Add(arena);
    }
}