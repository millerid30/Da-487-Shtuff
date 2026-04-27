using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float damage;
    private float sauce;
    public WeaponSO weapon;
    [TextArea]
    [SerializeField] private string weaponDescription;
    public WeaponType weaponType;
    private GameObject prefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if(iDamageable != null)
            {
                iDamageable.Damage(damage * weapon.powerMulti);
            }
        }
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