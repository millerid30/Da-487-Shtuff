using UnityEngine;

public class Pepperonipede : Enemy, IDamageable, IEnemyAttack1, IEnemyAttack2, IEnemyAttack3, IBossAttack1
{
    [Header("Segments")]
    public GameObject segmentPrefab;
    public int segments = 10;
    public bool isHead = false;
    public bool isTail = false;

    [Header("Movement")]
    [SerializeField] private float turnSpeed = 20f;
    [Range(0.01f, 1f)]
    [SerializeField] private float tailSpeedMulti = 0.95f;
    private float timer;

    public Transform aim;
    public Transform tail;
    public Transform follow;
    private GameObject seg;

    public Transform childObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        difficulty = DifficultyController.Instance.difficulty;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        player = GameObject.FindGameObjectWithTag("Player");
        quest = GameObject.FindAnyObjectByType<QuestController>();
        maxHealth = enemy.maxHealth * difficulty;
        health = maxHealth;
        isStunned = false;
        isDead = false;
        numDrops = Mathf.RoundToInt(1 + enemy.enemyNumDrops * difficulty / 10);
        if (transform.parent != null)
        {
            follow = transform.parent.GetComponent<Pepperonipede>().GiveTail();
            transform.position = follow.position;
        }
        segments *= Mathf.RoundToInt(difficulty);
        CreateChildren(segments);
        childObject = transform.GetChild(transform.childCount - 1);
        UpdateSegment();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        timer += Time.deltaTime;
        UpdateSegment();

        if (player != null && health > 0)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Move();
        }

        if (health <= 0)
        {
            if (!isDead && childObject != null && childObject.GetComponent<Pepperonipede>() != null)
            {
                childObject.parent = null;
            }
            StartCoroutine(OnDeath());
        }
    }

    public void UpdateSegment()
    {
        isHead = (follow == null);
        if (childObject == null)
        {
            isTail = true;
        }
        else if (childObject.GetComponent<Pepperonipede>() == null)
        {
            isTail = true;
        }
        else
        {
            isTail = false;
        }
    }
    protected override void Move()
    {
        Vector2 direction = player.transform.position - transform.position;
        if (isHead)
        {
            Turn();
            if (distance > minDistance)
            {
                rb.AddForce(aim.transform.up * enemy.moveSpeed * Time.deltaTime * 50);
            }
            //else if (distance < minDistance)
            //{
            //    transform.position = Vector2.Lerp(transform.position, -direction.normalized * enemy.moveSpeed * 100, enemy.moveSpeed * Time.deltaTime);
            //}
            else
            {
                // circle script
                // Circle();
                rb.AddForce(aim.transform.up * enemy.moveSpeed * Time.deltaTime * 25);
            }
        }
        else
        {
            Follow();
            transform.position = Vector2.Lerp(transform.position, follow.transform.position, enemy.moveSpeed * Time.deltaTime * 25);
        }
    }
    void Turn()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.back);
        aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, target, Time.deltaTime * turnSpeed);
    }
    void Follow()
    {
        aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, follow.rotation, Time.deltaTime * turnSpeed * tailSpeedMulti);
    }
    void CircleTimer()
    {

    }
    void Circle()
    {
        // circle around player
    }
    public void BossAttack1()
    {

    }

    public void SetSegments(int segments)
    {
        this.segments = segments;
    }
    public void CreateChildren(int segments)
    {
        if (segments > 0)
        {
            segments--;
            seg = GameObject.Instantiate(segmentPrefab, transform);
            seg.transform.localPosition = Vector2.zero;
            Pepperonipede newP = seg.GetComponentInChildren<Pepperonipede>();
            if (newP != null)
            {
                newP.SetSegments(segments);
            }
        }
    }
    public Transform GiveTail()
    {
        return tail;
    }
}