using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask whatDestroysProjectile;
    public ProjectileSO proj;
    private float sauce;
    private float sauceMulti;
    private Rigidbody2D rb;
    private Weapon weaponParent;
    private float logBase = 10;
    private float logMulti = 3;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInParent<Weapon>();
        SetVelocity();
        SetDestroyTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatDestroysProjectile.value & (1 << collision.gameObject.layer)) > 0)
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(sauce * sauceMulti);
            }
            if (collision.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D obj = collision.GetComponent<Rigidbody2D>();
                Knockback(obj, Mathf.Log(sauce, logBase) * logMulti);
            }
            Destroy(transform.parent.gameObject);
        }
    }
    public void Knockback(Rigidbody2D rb, float force)
    {
        Vector2 dir = (rb.transform.position - transform.position).normalized;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
    void SetVelocity()
    {
        rb.linearVelocity = transform.up * proj.speed;
    }
    void SetDestroyTime()
    {
        Destroy(transform.parent.gameObject, proj.lifespan);
    }
    public void SetSauce(float sauce)
    {
        this.sauce = sauce;
    }
    public void SetSauceMulti(float multi)
    {
        sauceMulti = multi;
    }
}