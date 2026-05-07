using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IBumpable, IStunnable
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 worldPosition;
    private Vector2 direction;

    private InputAction playerControls;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float drag = 1.0f;

    [Range(0.01f, 1.0f)]
    [SerializeField] private float kbResist = 0.01f;
    [Range(0.01f, 1.0f)]
    [SerializeField] private float stunResist = 0.01f;
    [SerializeField] private bool isStunned;

    [SerializeField] private Transform Aim;
    bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isStunned = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveInput == Vector2.zero)
        {
            isMoving = false;
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, drag, Time.deltaTime * moveSpeed * 5);
        }
        else
        {
            isMoving = true;
            rb.linearDamping = drag / 4;
        }
        rb.AddForce(moveInput.normalized * moveSpeed * Time.deltaTime * 150);

        if (lookInput != Vector2.zero)
        {
            direction = lookInput.normalized;
            Aim.transform.up = -direction;
        }
        else if (isMoving && Mouse.current.delta.ReadValue() == Vector2.zero)
        {
            Vector3 vector3 = Vector3.left * moveInput.x + Vector3.down * moveInput.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (worldPosition - (Vector2)Aim.transform.position).normalized;
            Aim.transform.up = -direction;
        }
    }
    public void Knockback(Vector2 direction, float force)
    {
        Vector2 dir = direction.normalized;
        rb.AddForce(direction * force * (1 - kbResist), ForceMode2D.Impulse);
    }
    public IEnumerator Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            yield return new WaitForSeconds(duration * (1 - stunResist));
            isStunned = false;
            yield break;
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        if (!isStunned)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}