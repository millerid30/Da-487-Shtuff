using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 worldPosition;
    private Vector2 direction;

    private InputAction playerControls;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float drag = 1.0f;

    [SerializeField] private Transform Aim;
    bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveInput == Vector2.zero)
        {
            isMoving = false;
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, drag, Time.deltaTime * moveSpeed);
        }
        else
        {
            isMoving = true;
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
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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