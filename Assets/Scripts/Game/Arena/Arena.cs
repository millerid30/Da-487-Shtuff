using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Arena : MonoBehaviour
{
    [SerializeField] private Transform[] enemySpawnpoints;
    [SerializeField] private Transform tipSpawn;
    private bool isComplete;
    private bool isRemoving;
    private Rigidbody2D rb;
    private Collider2D coll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        tipSpawn = GetComponent<Transform>();
        //SetSpawnLocations();
        //WaveSpawner.Instance.SetEnemySpawns();
    }

    // Update is called once per frame
    void Update()
    {
        if (isComplete && !isRemoving)
        {
            StartCoroutine(RemoveArena(3));
        }

        if (Keyboard.current.homeKey.wasPressedThisFrame)
        {
            isComplete = true;
        }
    }
    public bool IsComplete()
    {
        return isComplete;
    }
    public IEnumerator RemoveArena(float destroyTime)
    {
        this.tag = "OldArena";
        isRemoving = true;
        ArenaManager.Instance.NewArena();
        rb.constraints = RigidbodyConstraints2D.None;
        coll.enabled = false;
        rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
        yield break;
    }
    public int GetSpawnCount()
    {
        return enemySpawnpoints.Count();
    }
    public void SetSpawnLocations()
    {
        for (int i = 0; i < enemySpawnpoints.Length; i++)
        {
            ArenaManager.Instance.SpawnLocation(i).position = enemySpawnpoints[i].position;
        }
    }
    public Transform GetSpawnLocation(int index)
    {
        if (index >= enemySpawnpoints.Length) { return null; }
        return enemySpawnpoints[index];
    }
    public Transform[] GetEnemySpawns()
    {
        return enemySpawnpoints;
    }
    public Transform GetTipSpawn()
    {
        return tipSpawn;
    }
}