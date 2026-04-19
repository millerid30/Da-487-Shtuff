using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
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
    void Update()
    {
        //if (PauseController.IsGamePaused)
        //{
        //    rb.linearVelocity = Vector2.zero;
        //    return;
        //}

        //rb.linearVelocity = (moveInput.normalized * moveSpeed);
        if (moveInput == Vector2.zero)
        {
            isMoving = false;
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, drag, Time.deltaTime * 3);
        }
        else
        {
            isMoving = true;
        }
        rb.AddForce(moveInput.normalized * moveSpeed);

        if (isMoving)
        {
            Vector3 vector3 = Vector3.left * moveInput.x + Vector3.down * moveInput.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}