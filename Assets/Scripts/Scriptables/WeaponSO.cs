using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public float powerMulti, atkSpeedMulti, sauceMulti;
    public float comboMulti, finisherMulti;
    public int comboLength;
    public GameObject sauceProjectile;
}