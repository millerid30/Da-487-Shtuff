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
        if (collision.tag != "Player")
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            IBumpable iBumpable = collision.gameObject.GetComponent<IBumpable>();
            IStunnable iStunnable = collision.gameObject.GetComponent<IStunnable>();
            Rigidbody2D objRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (iDamageable != null)
            {
                iDamageable.Damage(damage * weapon.powerMulti);
                CinemachineShake.Instance.Shake(weapon.powerMulti, Mathf.Log(damage * weapon.powerMulti, logBase * logMulti) / (logMulti * weapon.atkSpeedMulti));
            }
            if (iBumpable != null)
            {
                if (objRB != null)
                {
                    Vector2 direction = (objRB.transform.position - transform.position).normalized;
                    iBumpable.Knockback(direction, Mathf.Log(damage, logBase) * logMulti);
                }
            }
            if (iStunnable != null)
            {
                StartCoroutine(iStunnable.Stun(Mathf.Log(damage, logBase)));
            }
        }
    }
    public void SauceItUp()
    {
        prefab = Object.Instantiate(weapon.sauceProjectile, transform.position, transform.rotation);
        Projectile newP = prefab.GetComponentInChildren<Projectile>();
        if (newP != null)
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