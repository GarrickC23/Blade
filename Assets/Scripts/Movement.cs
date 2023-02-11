using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;

    private PlayerControls playerControls;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Ground.Jump.performed += Jump; 
        playerControls.Ground.Parry.performed += Parry;
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void onDisable(){
        playerControls.Disable(); 
    }

    void Start()
    {

    }
    
    private void Update() {
        //Gets the WASD/Arrow Keys from Input System as a Vector2 (x, y)
        Vector2 move = playerControls.Ground.Movement.ReadValue<Vector2>();
        Debug.Log(move); //Implement Movement
        rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
    }

    //Jump Function. When you press "Space" you will jump
    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if ( context.performed )
        {
            Debug.Log("Jump!"); //Implement Jump 
            rb.velocity += new Vector2(0, jumpForce);
        }
    }

    //Parry Function. When you press "R" you will parry
    private void Parry(InputAction.CallbackContext context)
    {
        if ( context.performed )
        {
            Debug.Log("Parry!"); //Implement Parry Mechanic
        }
    }
}
