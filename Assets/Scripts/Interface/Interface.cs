using System.Collections;
using UnityEngine;

public interface IDamageable
{
    public void Damage(float damage);
}
public interface IBumpable
{
    public void Knockback(Vector2 direction, float force);
}
public interface IStunnable
{
    public IEnumerator Stun(float duration);
}
public interface IHitstop
{
    public IEnumerator Hitstop(float duration);
}
public interface IEnemyAttack1
{
    public IEnumerator EnemyAttack1();
}
public interface IEnemyAttack2
{
    public IEnumerator EnemyAttack2();
}
public interface IEnemyAttack3
{
    public IEnumerator EnemyAttack3();
}
public interface IBossAttack1
{
    public IEnumerator BossAttack1();
}