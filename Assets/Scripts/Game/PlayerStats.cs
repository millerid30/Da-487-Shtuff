using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health, speed, power, atkSpeed, sauce;
    [SerializeField] private TMP_Text hpText, spdText, pwrText, dpsText, sauceText;

    private GameObject Player;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerAttack attack;
    private Weapon damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("Player");
        playerHealth = Player.GetComponent<PlayerHealth>();
        playerMovement = Player.GetComponent<PlayerMovement>();
        attack = Player.GetComponent<PlayerAttack>();
        damage = Player.GetComponentInChildren<Weapon>(true);
        UpdateStats();
    }
    public void UpdateStats()
    {
        if (Player != null)
        {
            health = Mathf.RoundToInt(health);
            playerHealth.SetMaxHealth(health);
            playerMovement.SetMoveSpeed(speed);
            damage.SetDamage(power);
            attack.SetAttackSpeed(atkSpeed);
            damage.SetSauce(sauce);
        }
        hpText.text = health.ToString("0.##");
        spdText.text = speed.ToString("0.##");
        pwrText.text = power.ToString("0.##");
        dpsText.text = atkSpeed.ToString("0.##");
        sauceText.text = sauce.ToString("0.##");
    }
}