using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] Melee;
    private InputAction playerControls;
    private PlayerStats playerStats;
    public bool isAttacking = false;
    [SerializeField] private float atkDuration = 0.3f;
    [SerializeField] private float atkTimer = 0f;
    [SerializeField] private float comboDuration = 0.75f;
    [SerializeField] private float comboTimer = 0f;
    [SerializeField] private int comboCounter = 0;
    private int select = 0;

    private void Awake()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        foreach (GameObject m in Melee)
        {
            SetDamageTypes();
            foreach (Transform c in m.transform)
            {
                c.gameObject.SetActive(false);
            }
            m.SetActive(false);
        }
    }

    void Update()
    {
        MeleeTimer();
        ComboTimer();
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            Melee[select].SetActive(true);

            for (int i = 0; i < Melee[select].transform.childCount; i++)
            {
                Melee[select].transform.GetChild(i).gameObject.SetActive(i == comboCounter);
            }

            if (Melee[select].GetComponent<Weapon>().weaponType == WeaponType.Sauce)
            {
                Melee[select].GetComponent<Weapon>().SauceItUp();
            }
            isAttacking = true;
            comboTimer = 0f;
            comboCounter++;
            if (comboCounter >= Melee[select].GetComponent<Weapon>().weapon.comboLength)
            {
                comboCounter = 0;
            }
        }
    }

    void MeleeTimer()
    {
        if (isAttacking)
        {
            if (Melee[select].GetComponent<Weapon>() != null)
            {
                //if (Melee[select].GetComponent<Weapon>().weapon.comboLength - 1 == comboCounter)
                //{
                //    atkTimer += (Time.deltaTime * Melee[select].GetComponent<Weapon>().weapon.atkSpeedMulti) / (0.5f * Melee[select].GetComponent<Weapon>().weapon.finisherMulti);
                //}
                //else
                //{
                atkTimer += Time.deltaTime * Melee[select].GetComponent<Weapon>().weapon.atkSpeedMulti;
                //}

            }
            else
            {
                atkTimer += Time.deltaTime;
            }
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
                foreach (GameObject m in Melee)
                {
                    m.SetActive(false);
                }
            }
        }
    }
    void ComboTimer()
    {
        comboTimer += Time.deltaTime;
        if (comboTimer >= comboDuration)
        {
            comboTimer = 0;
            comboCounter = 0;
        }
    }
    public void PrevWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            select--;
            select = (select + Melee.Length) % Melee.Length;
            SetDamageTypes();
        }
    }
    public void NextWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            select++;
            select %= Melee.Length;
            SetDamageTypes();
        }
    }
    public int GetSelectedWeapon()
    {
        return select;
    }
    public void SetAttackSpeed(float speed)
    {
        atkDuration = 1 / speed;
    }
    public void SetDamageTypes()
    {
        if (Melee[select].GetComponent<Weapon>() != null)
        {
            Melee[select].GetComponent<Weapon>().SetDamage(playerStats.power);
            Melee[select].GetComponent<Weapon>().SetSauce(playerStats.sauce);
        }
    }
}