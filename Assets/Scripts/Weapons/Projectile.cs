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
            IBumpable iBumpable = collision.gameObject.GetComponent<IBumpable>();
            IStunnable iStunnable = collision.gameObject.GetComponent<IStunnable>();
            Rigidbody2D objRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (iDamageable != null)
            {
                iDamageable.Damage(sauce * sauceMulti);
                CinemachineShake.Instance.Shake(1 + proj.damage / proj.speed, 0.1f + (0.05f * sauceMulti) + (0.05f / proj.speed));
            }
            if (iBumpable != null)
            {
                if (objRB != null)
                {
                    Vector2 direction = (objRB.transform.position - transform.position).normalized;
                    iBumpable.Knockback(direction, Mathf.Log(sauce, logBase) * logMulti);
                }
            }
            if (iStunnable != null)
            {
                StartCoroutine(iStunnable.Stun(Mathf.Log(sauce, logBase)));
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