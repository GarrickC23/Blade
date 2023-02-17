using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public float moveSpeed;

    [Header("Jumping")] 
    private float jumpForce;                // How fast the player moves upwards initially
    public float jumpHeight;                // How high the player jumps
    public float jumpTime;                  // How long it takes the player to reach the height of their jump
    public float downwardsForce;            // How much the player slows down at apex of jump
    public float fastFallForce;

    [Header("Running")]
    public float acceleration; 
    public float decceleration; 
    public float minSpeed; 
    public float maxSpeed; 

    private PlayerControls playerControls;
    private bool canJump;
    private bool canFastFall;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Ground.Jump.performed += Jump; 
        playerControls.Ground.FastFall.performed += FastFall;
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void onDisable(){
        playerControls.Disable(); 
    }

    void Start()
    {
        canJump = true;
        canFastFall = false;
        float gravityStrength = -(2 * jumpHeight) / (jumpTime * jumpTime);
        float gravityScale = gravityStrength / Physics2D.gravity.y;
        rb.gravityScale = gravityScale;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTime;
    }
    
    private void Update() {
        Move();
    }

    private void Move() {
        //Gets the WASD/Arrow Keys from Input System as a Vector2 (x, y)
        Vector2 move = playerControls.Ground.Movement.ReadValue<Vector2>();
        // Debug.Log(move); //Implement Movement
        rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
        if ( rb.velocity.x > 0 && moveSpeed < maxSpeed )
        {
            moveSpeed += acceleration * Time.deltaTime; 
        }
        else if ( rb.velocity.x < 0 && moveSpeed < maxSpeed )
        {
            moveSpeed += acceleration * Time.deltaTime; 
        }

        if ( move.x == 0 && moveSpeed > 5 )
        {
            moveSpeed -= decceleration * Time.deltaTime; 
        }
    }

    //Jump Function. When you press "Space" you will jump
    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if ( context.performed )
        {
            if (canJump) {
                Debug.Log("Jump!"); //Implement Jump 
                canJump = false;
                StartCoroutine(Jump());
            }
        }
    }

    private IEnumerator Jump() {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpTime);
        canFastFall = true;
        rb.AddForce(new Vector2(0, -downwardsForce));
    }


    private void FastFall(InputAction.CallbackContext context) {
        if (context.performed) {
            if (canFastFall) {
                Debug.Log("Fastfalling");
                rb.AddForce(new Vector2(0, -fastFallForce));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            canJump = true;
            canFastFall = false;
        }
    }
}
