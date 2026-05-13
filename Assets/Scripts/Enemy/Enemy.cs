using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IBumpable, IStunnable, IEnemyAttack1, IEnemyAttack2, IEnemyAttack3
{
    protected private Rigidbody2D rb;
    public EnemySO enemy;
    protected private float difficulty = 1;
    public bool isStunned;
    [Header("Health")]
    [SerializeField] protected private float health;
    protected private float maxHealth = 1;

    [Header("Distance")]
    [SerializeField] protected private Transform aim;
    [SerializeField] protected private float maxDistance = 7f;
    [SerializeField] protected private float minDistance = 2f;
    [SerializeField] protected private float decisionDelay = 3f;
    float wanderTimer = 0f;
    bool isWandering = true;
    //protected bool isStunned;
    protected float attackTimer = 0f;
    protected bool isAttacking = false;

    [Range(0.01f, 1.0f)]
    [SerializeField] protected private float kbResist = 0.01f;
    [Range(0.01f, 1.0f)]
    [SerializeField] protected private float stunResist = 0.01f;

    [Header("Silly")]
    [Range(0f, 100f)]
    [SerializeField] protected private float sillyCoefficient = 50f;
    [Range(0f, 3f)]
    [SerializeField] protected private float spawnDelay = 0.125f;

    protected private float[] Weights;
    protected private int numDrops = 1;

    protected private GameObject player;
    private GameObject prefab;
    protected private float distance;
    protected private QuestController quest;
    protected private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        difficulty = DifficultyController.Instance.difficulty;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        Physics2D.IgnoreLayerCollision(7, 6, false);
        player = GameObject.FindGameObjectWithTag("Player");
        quest = GameObject.FindAnyObjectByType<QuestController>();
        maxHealth = enemy.maxHealth * difficulty;
        health = maxHealth;
        isStunned = false;
        isDead = false;
        numDrops = Mathf.RoundToInt(1 + enemy.enemyNumDrops * difficulty / 10);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        WanderTimer();
        AttackTimer();
        if (!isStunned)
        {
            if (player != null && health > 0)
            {
                distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= maxDistance)
                {
                    Move();
                }
                else
                {
                    Wander();
                }
            }
        }
        if (health <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        //Hitstop.Instance.Stop(0.1f);
    }
    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    protected virtual void Move()
    {
        Vector2 direction = player.transform.position - transform.position;

        if (distance > minDistance)
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position, enemy.moveSpeed * Time.deltaTime / 2);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position, enemy.moveSpeed * Time.deltaTime / 6);
            StartCoroutine(EnemyAttack1());
        }
    }
    void WanderTimer()
    {
        if (isWandering)
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= decisionDelay)
            {
                wanderTimer = 0;
                isWandering = false;
            }
        }
    }
    void Wander()
    {
        if (!isWandering)
        {
            isWandering = true;
            float angle = Random.Range(0f, 360f);
            rb.linearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * enemy.moveSpeed * 50 * Time.deltaTime;
        }
    }
    protected virtual void AttackTimer()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= decisionDelay)
            {
                attackTimer = 0;
                isAttacking = false;
            }
        }
    }
    public IEnumerator EnemyAttack1()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            yield return new WaitForSeconds(1);
            Vector2 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            aim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            rb.AddForce(aim.transform.up * enemy.moveSpeed, ForceMode2D.Impulse);
        }
        yield break;
    }
    public IEnumerator EnemyAttack2()
    {
        Debug.Log("Implement Attack");
        yield break;
    }
    public IEnumerator EnemyAttack3()
    {
        Debug.Log("Implement Attack");
        yield break;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.tag == "Player")
            {
                IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
                IBumpable iBumpable = collision.gameObject.GetComponent<IBumpable>();
                IStunnable iStunnable = collision.gameObject.GetComponent<IStunnable>();
                Rigidbody2D objRB = collision.gameObject.GetComponent<Rigidbody2D>();
                if (iDamageable != null)
                {
                    iDamageable.Damage(enemy.power);
                }
                if (iBumpable != null)
                {
                    if (objRB != null)
                    {
                        Vector2 direction = (objRB.transform.position - transform.position).normalized;
                        iBumpable.Knockback(direction, enemy.power);
                    }
                }
                if (iStunnable != null)
                {
                    StartCoroutine(iStunnable.Stun(Mathf.Log10(enemy.power)));
                }
            }

        }
    }

    protected virtual IEnumerator OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            Physics2D.IgnoreLayerCollision(7, 6, true);
            SendDeathMessage();
            rb.freezeRotation = false;
            var randForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * sillyCoefficient;
            rb.AddForce(randForce, ForceMode2D.Impulse);
            rb.AddTorque(randForce.x / 2, ForceMode2D.Impulse);
            yield return new WaitForSeconds(spawnDelay);
            if (enemy.enemyGiblets != null)
            {
                Instantiate(enemy.enemyGiblets, transform.position, transform.rotation);
            }
            EnemyItemSpawner();
            Destroy(gameObject, 3f);
            yield break;
        }
    }
    public void SendDeathMessage()
    {
        if (quest == null) { return; }
        foreach (QuestProgress questP in QuestController.Instance.activateQuests)
        {
            foreach (QuestObjective questO in questP.objectives)
            {
                if (questO.objectiveID.Equals(enemy.enemyID))
                {
                    questO.amount++;
                }
            }
        }

        QuestController.Instance.questUI.UpdateQuestUI();
        DifficultyController.Instance.enemiesDefeated++;
        DifficultyController.Instance.IncreaseDifficulty(enemy.threatLevel);
    }
    public void Knockback(Vector2 direction, float force)
    {
        Vector2 dir = direction.normalized;
        rb.AddForce(direction * force * (1 - kbResist), ForceMode2D.Impulse);
    }

    public IEnumerator Stun(float duration)
    {
        if (!isStunned)
        {
            //movement = 0;
            isStunned = true;
            yield return new WaitForSeconds(duration * (1 - stunResist));
            // allow movement;
            isStunned = false;
            yield break;
        }
    }
    public void EnemyItemSpawner()
    {
        Weights = new float[enemy.enemyDrops.Count];
        for (int j = 0; j < numDrops; j++)
        {
            float value = Random.value;
            ResetSpawnWeights();
            for (int i = 0; i < Weights.Length; i++)
            {
                if (value < Weights[i])
                {

                    prefab = Instantiate(enemy.enemyDrops[i].spawn, transform.position, Quaternion.identity);
                    Rigidbody2D prb = prefab.GetComponent<Rigidbody2D>();
                    if (prb != null)
                    {
                        var randForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2;
                        prb.AddForce(randForce, ForceMode2D.Impulse);
                        prb.AddTorque(randForce.x, ForceMode2D.Impulse);
                    }
                    break;
                }
                value -= Weights[i];
            }
        }
    }
    void ResetSpawnWeights()
    {
        float totalWeight = 0;
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] = enemy.enemyDrops[i].GetWeight();
            totalWeight += Weights[i];
        }
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] /= totalWeight;
        }
    }
}