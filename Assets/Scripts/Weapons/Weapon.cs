using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float damage;
    private float sauce;
    public WeaponSO weapon;
    private float logBase = 10;
    private float logMulti = 10;
    [TextArea]
    [SerializeField] private string weaponDescription;
    public WeaponType weaponType;
    private GameObject prefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Boss")
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if(iDamageable != null)
            {
                iDamageable.Damage(damage * weapon.powerMulti);
            }
            if(collision.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D obj = collision.GetComponent<Rigidbody2D>();
                Knockback(obj,Mathf.Log(damage,logBase)*logMulti);
            }
        }
    }
    public void Knockback(Rigidbody2D rb,float force)
    {
        Vector2 dir = (rb.transform.position - transform.position).normalized;
        rb.AddForce(dir * force,ForceMode2D.Impulse);
    }
    public void SauceItUp()
    {
        prefab = Object.Instantiate(weapon.sauceProjectile, transform.position, transform.rotation);
        Projectile newP = prefab.GetComponentInChildren<Projectile>();
        if(newP != null)
        {
            newP.SetSauce(sauce);
            newP.SetSauceMulti(weapon.sauceMulti);
        }
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetSauce(float sauce)
    {
        this.sauce = sauce;
    }
    
}
public enum WeaponType
{
    Power, Sauce, Hybrid
};