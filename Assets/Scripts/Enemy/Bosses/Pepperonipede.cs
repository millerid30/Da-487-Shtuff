using System.Collections;
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

    [Header("Attack")]
    [SerializeField] private Vector3 attackPoint;
    [SerializeField] private float angleSpeed;
    [SerializeField] private float radiusSpeed;
    [SerializeField] private float circleRadius;
    [SerializeField] private float circleAngle = 0f;
    [Range(1f, 50f)]
    [SerializeField] private float chargeForce = 15f;

    [Header("Transforms")]
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
        if (segments % 2 == 0)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
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

        attackPoint = transform.position;
        angleSpeed = turnSpeed * 5;
        radiusSpeed = enemy.moveSpeed * 2;
        circleRadius = minDistance;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        timer += Time.deltaTime;
        AttackTimer();
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
        if (follow == null)
        {
            if (!isHead)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHead = true;
            }
        }
        if (childObject == null)
        {
            isTail = true;
        }
        else if (childObject.GetComponent<Pepperonipede>() == null)
        {
            isTail = true;
        }
        if (!isHead)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    protected override void Move()
    {
        Vector2 direction = player.transform.position - transform.position;
        if (isHead)
        {
            if (!isAttacking)
            {
                Turn();
                if (distance > maxDistance && attackTimer <= 0)
                {
                    StartCoroutine(ChargeAttack(chargeForce));
                }
                else if (distance > minDistance)
                {
                    rb.AddForce(aim.transform.up * enemy.moveSpeed * Time.deltaTime * 50);
                }
                else
                {
                    float value = Random.value;
                    if (value <= 0.45f)
                    {
                        StartCoroutine(CircleAttack());
                    }
                    else if (value <= 0.90f)
                    {
                        StartCoroutine(ChargeAttack(chargeForce / 3));
                    }
                    else
                    {
                        // Do Attack 3
                        if (!isAttacking)
                        {
                            isAttacking = true;
                        }
                    }
                }
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
        aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, follow.rotation, Time.deltaTime * turnSpeed * tailSpeedMulti * 2.5f);
    }
    IEnumerator ChargeAttack(float force)
    {
        if (!isAttacking)
        {
            isAttacking = true;

            attackPoint = player.transform.position;
            Vector2 dir = attackPoint - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            aim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            yield return new WaitForSeconds(1);
            rb.AddForce(aim.transform.up * force, ForceMode2D.Impulse);
        }
        yield break;
    }
    IEnumerator CircleAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            yield return new WaitForSeconds(1);
            attackPoint = player.transform.position;
            circleAngle = Mathf.Atan2(attackPoint.y - transform.position.y, attackPoint.x - transform.position.x) * Mathf.Rad2Deg;

            while (circleRadius > 0)
            {
                isAttacking = true;
                circleAngle += angleSpeed * Time.deltaTime;
                circleRadius -= radiusSpeed * Time.deltaTime;

                float x = attackPoint.x + Mathf.Cos(circleAngle) * circleRadius;
                float y = attackPoint.y + Mathf.Sin(circleAngle) * circleRadius;
                Vector3 dir = Vector3.Cross(transform.position - attackPoint, Vector3.back);
                float ang = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                aim.transform.rotation = Quaternion.Euler(0, 0, -ang);
                transform.position = new Vector2(x, y);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            circleRadius = minDistance;
        }
        yield break;
    }
    public IEnumerator BossAttack1()
    {
        yield break;
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