using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask whatDestroysProjectile;
    public ProjectileSO proj;
    private float sauce;
    private float sauceMulti;
    private Rigidbody2D rb;
    private Weapon weaponParent;
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
            Destroy(transform.parent.gameObject);
        }
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