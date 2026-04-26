using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public float powerMulti, atkSpeedMulti, sauceMulti;
    public GameObject sauceProjectile;
}