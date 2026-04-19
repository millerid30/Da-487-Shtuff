using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if(iDamageable != null)
            {
                iDamageable.Damage(damage);
            }
        }
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}