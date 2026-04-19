using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // TEST damage/heal
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Damage(20);
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Heal(10);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    public void SetMaxHealth(float maxHealth)
    {
        health += maxHealth - this.maxHealth;
        this.maxHealth = maxHealth;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}