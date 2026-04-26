using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] Melee;
    private InputAction playerControls;
    public bool isAttacking = false;
    [SerializeField] private float atkDuration = 0.3f;
    [SerializeField] private float atkTimer = 0f;
    private int select = 0;

    [SerializeField] private InputActionReference attack;

    private void Awake()
    {
        foreach (GameObject m in Melee)
        {
            m.SetActive(false);
        }
    }

    private void OnEnable()
    {
        attack.action.performed += Attack;
    }
    private void OnDisable()
    {
        attack.action.performed -= Attack;
    }

    void Update()
    {
        MeleeTimer();
        
    }
    void Attack(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            Melee[select].SetActive(true);
            if (Melee[select].GetComponent<Weapon>() != null && Melee[select].GetComponent<Weapon>().weaponType == WeaponType.Sauce)
            {
                Melee[select].GetComponent<Weapon>().SauceItUp();
            }
            isAttacking = true;
        }
    }

    void MeleeTimer()
    {
        if (isAttacking)
        {
            if (Melee[select].GetComponent<Weapon>() != null)
            {
                atkTimer += Time.deltaTime * Melee[select].GetComponent<Weapon>().weapon.atkSpeedMulti;
            }
            else
            {
                atkTimer += Time.deltaTime;
            }
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
                Melee[select].SetActive(false);
            }
        }
    }
    public void PrevWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            select--;
            if (select < 0)
            {
                select = Melee.Length - 1;
            }
        }
    }
    public void NextWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            select++;
            if (select >= Melee.Length)
            {
                select = 0;
            }
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
}