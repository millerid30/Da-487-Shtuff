using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IBumpable, IStunnable, IEnemyAttack1, IEnemyAttack2, IEnemyAttack3
{
    protected private Rigidbody2D rb;
    public EnemySO enemy;

    [Header("Health")]
    [SerializeField] protected private float health;

    [Header("Distance")]
    [SerializeField] protected private float maxDistance = 7f;
    [SerializeField] protected private float minDistance = 2f;
    [SerializeField] protected private float decisionDelay = 2f;
    float wanderTimer = 0f;
    bool isWandering = true;
    bool isStunned;

    [Header("Silly")]
    [Range(0f, 100f)]
    [SerializeField] protected private float sillyCoefficient = 50f;
    [Range(0f, 3f)]
    [SerializeField] protected private float spawnDelay = 0.125f;

    protected private GameObject player;
    private GameObject prefab;
    protected private float distance;
    protected private QuestController quest;
    private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        player = GameObject.FindGameObjectWithTag("Player");
        quest = GameObject.FindAnyObjectByType<QuestController>();
        isStunned = false;
        isDead = false;
        health = enemy.maxHealth;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        WanderTimer();
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
    }
    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, enemy.maxHealth);
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
            float angle = Random.Range(0f, 360f);
            rb.linearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * enemy.moveSpeed * 50 * Time.deltaTime;
            isWandering = true;
        }
    }

    public void EnemyAttack1()
    {
        Debug.Log("Implement Attack");
    }
    public void EnemyAttack2()
    {
        Debug.Log("Implement Attack");
    }
    public void EnemyAttack3()
    {
        Debug.Log("Implement Attack");
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
                    iStunnable.Stun(Mathf.Log10(enemy.power));
                }
            }

        }
    }

    protected virtual IEnumerator OnDeath()
    {
        if (!isDead)
        {
            SendDeathMessage();
            isDead = true;
            rb.freezeRotation = false;
            var randForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * sillyCoefficient;
            rb.AddForce(randForce, ForceMode2D.Impulse);
            rb.AddTorque(randForce.x, ForceMode2D.Impulse);
            yield return new WaitForSeconds(spawnDelay);
            if (enemy.enemyGiblets != null)
            {
                Instantiate(enemy.enemyGiblets, transform.position, transform.rotation);
            }
            if (enemy.enemyDrops != null)
            {
                for (int i = 0; i < enemy.enemyNumDrops; i++)
                {
                    int randD = Random.Range(0, enemy.enemyDrops.Length);
                    Vector3 randL = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    prefab = Instantiate(enemy.enemyDrops[randD], transform.position + randL, transform.rotation);
                    prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f * 10f), Random.Range(-1f, 1f)) * sillyCoefficient / 10f);
                }
            }
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
    }
    public void Knockback(Vector2 direction, float force)
    {
        Vector2 dir = direction.normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public IEnumerator Stun(float duration)
    {
        if (!isStunned)
        {
            //movement = 0;
            isStunned = true;
            yield return new WaitForSeconds(duration);
            // allow movement;
            isStunned = false;
            yield break;
        }
    }
}