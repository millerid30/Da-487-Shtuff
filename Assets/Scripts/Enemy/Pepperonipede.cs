using UnityEngine;

public class Pepperonipede : Enemy, IDamageable, IEnemyAttack1, IEnemyAttack2, IEnemyAttack3, IBossAttack1
{
    [Header("Segments")]
    public GameObject segmentPrefab;
    public int segments = 10;
    public bool isHead = false;
    public bool isTail = false;

    [Header("Movement")]
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float lastPosTimer = 1f;
    private float timer;

    public Pepperonipede parentObject; // parent of this segment
    private GameObject seg;
    public Pepperonipede childObject; // child of this segment
    private Transform lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        health = enemy.maxHealth;
        // Set parent of this object to parentObject
        parentObject.GetComponentInParent<Transform>();
        lastPos = transform;
        UpdateSegment();
        CreateChildren(segments);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        UpdateSegment();
        if (timer >= lastPosTimer)
        {
            lastPos = transform;
            LastPositionTimer();
        }
        if (distance >= minDistance)
        {
            Move();
        }
        else
        {
            Circle();
        }

    }
    public void UpdateSegment()
    {
        if (parentObject == null)
        {
            isHead = true;
        }
        else if (childObject == null)
        {
            isTail = true;
        }
        else
        {
            isHead = false;
            isTail = false;
        }
    }
    void Move()
    {
        Vector2 direction = player.transform.position - transform.position;
        if (isHead)
        {
            Turn();
            if (distance > minDistance)
            {
                rb.AddForce(transform.up * enemy.moveSpeed * Time.deltaTime * 50);
            }
            //else if (distance < minDistance)
            //{
            //    transform.position = Vector2.Lerp(transform.position, -direction.normalized * enemy.moveSpeed * 100, enemy.moveSpeed * Time.deltaTime);
            //}
            else
            {
                // circle script
                // Circle();
                rb.AddForce(transform.up * enemy.moveSpeed * Time.deltaTime * 25);
            }
        }
        else
        {
            // follow parent
            Follow();
            transform.position = Vector2.Lerp(transform.position, parentObject.transform.position, enemy.moveSpeed * Time.deltaTime);
        }
    }
    void Turn()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSpeed / 10);
    }
    void Follow()
    {
        Vector2 direction = (parentObject.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSpeed / 10);
    }
    void LastPositionTimer()
    {
        timer = 0;
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
            // instantiate segment as child of this segment
            seg = GameObject.Instantiate(segmentPrefab, transform);
            // set this segment as parent of instantiated child
            seg.transform.SetParent(transform);
            Pepperonipede newP = seg.GetComponentInChildren<Pepperonipede>();
            if (newP != null)
            {
                newP.SetSegments(segments);
            }
        }
    }
}