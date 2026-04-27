using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Image hpBar;

    [Header("i-Frames")]
    [SerializeField] private float frames = 1f;
    [SerializeField] private LayerMask whatPlayerIgnores;
    private GameObject player;
    private Collider2D coll;
    private SpriteRenderer spr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        coll = player.GetComponent<Collider2D>();
        spr = player.GetComponent<SpriteRenderer>();
        SetHpBar();
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
        SetHpBar();
        StartCoroutine(IFrames(frames));
    }
    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
        SetHpBar();
    }
    public float GetHealth()
    {
        return health;
    }
    public void SetMaxHealth(float maxHealth)
    {
        health += maxHealth - this.maxHealth;
        this.maxHealth = maxHealth;
        health = Mathf.Clamp(health, 0, maxHealth);
        SetHpBar();
    }
    private void SetHpBar()
    {
        hpBar.fillAmount = health / maxHealth;
    }
    IEnumerator IFrames(float seconds)
    {
        // Ignore enemy layer
                                        //whatPlayerIgnores.value
        Physics2D.IgnoreLayerCollision(6, 7, true);
        // Lower alpha
        Color c = Color.white;
        c.a = 0.75f;
        spr.color = c;
        // Maybe flash or something
        yield return new WaitForSeconds(seconds);
        // Unignore layer
                                        //whatPlayerIgnores.value
        Physics2D.IgnoreLayerCollision(6, 7, false);
        // Alpha = 1
        c.a = 1f;
        spr.color = c;

        yield break;
    }
}