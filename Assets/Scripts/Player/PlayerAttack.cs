using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Melee;
    private InputAction playerControls;
    public bool isAttacking = false;
    [SerializeField] private float atkDuration = 0.3f;
    [SerializeField] private float atkTimer = 0f;

    [SerializeField] private InputActionReference attack;

    private void Awake()
    {
        Melee.SetActive(false);
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
            Melee.SetActive(true);
            isAttacking = true;
        }
    }

    void MeleeTimer()
    {
        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }
    public void SetAttackSpeed(float speed)
    {
        atkDuration = 1/speed;
    }
}