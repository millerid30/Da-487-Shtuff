using UnityEngine;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance { get; private set; }
    [SerializeField] private Transform spawnLocation;
    private Arena arena;

    private float[] Weights;
    private GameObject prefab;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        spawnLocation = GetComponent<Transform>();
    }
    private void Start()
    {
        //UpdateSpawn();
    }
    public void GiveQuestReward(QuestSO quest)
    {
        if (quest?.rewards == null) { return; }
        foreach (var reward in quest.rewards)
        {
            switch (reward.type)
            {
                case RewardType.Item:
                    ItemReward(reward, reward.amount);
                    break;
                case RewardType.Gold:
                    // Give Gold
                    break;
                case RewardType.Other:
                    // Give Other
                    break;
            }
        }
    }
    public void GiveQuestFail(QuestSO quest)
    {
        if (quest?.rewards == null) { return; }
        foreach (var fail in quest.rewards)
        {
            switch (fail.type)
            {
                case RewardType.FailEnemy:
                    EnemyPunishment(fail, fail.amount);
                    break;
                case RewardType.FailDifficulty:
                    DifficultyPunishment(fail.amount);
                    break;
            }
        }
    }
    public void ItemReward(QuestReward reward, int amount)
    {
        Weights = new float[reward.rewardPool.Count];
        for (int j = 0; j < amount; j++)
        {
            float value = Random.value;
            ResetSpawnWeights(reward);
            for (int i = 0; i < Weights.Length; i++)
            {
                if (value < Weights[i])
                {
                    if (spawnLocation != null)
                    {
                        prefab = Instantiate(reward.rewardPool[i].spawn, spawnLocation.position, spawnLocation.rotation);
                        Rigidbody2D prb = prefab.GetComponent<Rigidbody2D>();
                        if (prb != null)
                        {
                            var randForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2;
                            prb.AddForce(randForce, ForceMode2D.Impulse);
                            prb.AddTorque(randForce.x, ForceMode2D.Impulse);
                        }
                        break;
                    }
                }
                value -= Weights[i];
            }
        }
    }
    public void EnemyPunishment(QuestReward fail, int amount)
    {
        Weights = new float[fail.rewardPool.Count];
        for (int j = 0; j < amount; j++)
        {
            float value = Random.value;
            ResetSpawnWeights(fail);
            for (int i = 0; i < Weights.Length; i++)
            {
                if (value < Weights[i])
                {
                    Enemy enemy = fail.rewardPool[i].spawn.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        WaveSpawner.Instance.ManualSpawn(enemy.enemy.enemyID.ToString());
                    }
                    break;
                }
                value -= Weights[i];
            }
        }
    }
    public void DifficultyPunishment(int amount)
    {
        DifficultyController.Instance.IncreaseDifficulty(amount);
    }
    void ResetSpawnWeights(QuestReward reward)
    {
        float totalWeight = 0;
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] = reward.rewardPool[i].GetWeight();
            totalWeight += Weights[i];
        }
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] /= totalWeight;
        }
    }
    public void UpdateSpawn()
    {

    }
}