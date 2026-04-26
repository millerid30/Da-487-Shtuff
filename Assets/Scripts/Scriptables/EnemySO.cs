using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float maxHealth, moveSpeed, power, threatLevel;
    public GameObject[] enemyDrops;
    public int enemyNumDrops;
    public GameObject enemyGiblets;
}
public enum EnemyType
{
    Melee, Ranged, Hybrid
};