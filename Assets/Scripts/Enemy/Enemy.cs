using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyAttack1, IEnemyAttack2, IEnemyAttack3
{
    private Rigidbody2D rb;
    public EnemySO enemy;
    
    [Header("Health")]
    [SerializeField] private float health;

    [Header("Distance")]
    [SerializeField] private float maxDistance = 4f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float decisionDelay = 2f;
    float wanderTimer = 0f;
    bool isWandering = true;

    [Header("Silly")]
    [Range(0f, 200f)]
    [SerializeField] private float sillyCoefficient = 50f;

    private Transform player;
    private float distance;
    private GameObject dingding;
    private bool isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        isDead = false;
        health = enemy.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        WanderTimer();
        if (player != null && health > 0)
        {
            if (distance <= maxDistance)
            {
                Move();
            }
            else
            {
                Wander();
            }
        }
        if (health <= 0)
        {
            OnDeath();
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
    void Move()
    {
        Vector2 direction = player.transform.position - transform.position;

        if (distance > minDistance)
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position, enemy.moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position, enemy.moveSpeed * Time.deltaTime / 3);
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
            rb.linearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * enemy.moveSpeed * 200 * Time.deltaTime;
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

    void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            if (enemy.enemyGiblets != null)
            {
                Instantiate(enemy.enemyGiblets, transform.position, transform.rotation);
            }
            if (enemy.enemyDrops != null)
            {
                for (int i = 0; i < enemy.enemyNumDrops; i++)
                {
                    int randD = Random.Range(0, enemy.enemyDrops.Length);
                    var randL = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    dingding = Instantiate(enemy.enemyDrops[randD], transform.position + randL, transform.rotation);
                    var randF = new Vector2(Random.Range(-1f, 1f * 10f), Random.Range(-1f, 1f)) * sillyCoefficient / 10f;
                    dingding.GetComponent<Rigidbody2D>().AddForce(randF);
                }
            }
            var randForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * sillyCoefficient;
            rb.AddForce(randForce);
            rb.AddTorque(randForce.x * sillyCoefficient);
            Destroy(gameObject, 3f);
        }
    }
}