using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;

    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 30f;

    [SerializeField] private GameObject droppings;
    [SerializeField] private int numDrops;
    private GameObject dingding;
    private int count = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (count == 0)
            {
                for (int i = 0; i < numDrops; i++)
                {
                    var randL = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                    dingding = Instantiate(droppings, transform.position + randL, transform.rotation);
                }
                count++;
            }
            var randForce = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
            rb.AddForce(randForce);
            Destroy(gameObject, 3f);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}