using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public PlayerControls playerControls;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;

    private void Awake()
    {
        playerControls = new PlayerControls();

    }

    private void OnEnable()
    {
        move = playerControls.Ground.Movement;
        move.Enable();
        jump = playerControls.Ground.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void onDisable()
    {
        move.Disable();
        jump.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("We jumped");
    }
}
