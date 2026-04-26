using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public float health, speed, power, atkSpeed, sauce;
    
    public void EquipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerStats.health *= health;
        playerStats.speed *= speed;
        playerStats.power *= power;
        playerStats.atkSpeed *= atkSpeed;
        playerStats.sauce *= sauce;

        playerStats.UpdateStats();
    }
    public void UnequipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerStats.health /= health;
        playerStats.speed /= speed;
        playerStats.power /= power;
        playerStats.atkSpeed /= atkSpeed;
        playerStats.sauce /= sauce;

        playerStats.UpdateStats();
    }
}