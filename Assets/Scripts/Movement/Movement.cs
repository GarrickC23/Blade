using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private Animator anim;
    public float moveSpeed;

    [Header("Jumping")] 
    private float jumpForce;                // How fast the player moves upwards initially
    public float jumpHeight;                // How high the player jumps
    public float jumpTime;                  // How long it takes the player to reach the height of their jump
    public float downwardsForce;            // How much the player slows down at apex of jump
    public float fastFallForce;
    public float coyoteTime;
    private float coyoteTimer;

    [Header("Running")]
    public float acceleration; 
    public float decceleration; 
    public float minSpeed; 
    public float maxSpeed; 

    private PlayerControls playerControls;
    private bool canFastFall;
    
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>(); 
        playerControls = new PlayerControls();
        playerControls.Ground.Jump.performed += Jump; 
        playerControls.Ground.Jump.canceled += JumpRelease;
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
        canFastFall = false;
        float gravityStrength = -(2 * jumpHeight) / (jumpTime * jumpTime);
        float gravityScale = gravityStrength / Physics2D.gravity.y;
        rb.gravityScale = gravityScale;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTime;
    }
    
    private void Update() {
        Move();
        if (coyoteTimer < coyoteTime) {
            coyoteTimer += Time.deltaTime;
        }
    }

    private void Move() {
        //Gets the WASD/Arrow Keys from Input System as a Vector2 (x, y)
        Vector2 move = playerControls.Ground.Movement.ReadValue<Vector2>();
        // Debug.Log(move); //Implement Movement
        rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
        if ( rb.velocity.x > 0 && moveSpeed < maxSpeed )
        {
            moveSpeed += acceleration * Time.deltaTime; 
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            anim.Play("Run");
        }
        else if ( rb.velocity.x < 0 && moveSpeed < maxSpeed )
        {
            moveSpeed += acceleration * Time.deltaTime; 
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.Play("Run");
        }

        if ( move.x == 0 && moveSpeed > 4 )
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
            if (coyoteTimer < coyoteTime) {
                Debug.Log("Jump!"); //Implement Jump 
                anim.Play("HeroKnight_Jump");
                StartCoroutine(Jump());
            }
        }
    }

    private IEnumerator Jump() {
        coyoteTimer = coyoteTime;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpTime);
        canFastFall = true;
        rb.AddForce(new Vector2(0, -downwardsForce));
    }

    private void JumpRelease(InputAction.CallbackContext context)
    {
        if ( context.canceled )
        {
            if (rb.velocity.y > 0) 
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }


    private void FastFall(InputAction.CallbackContext context) {
        if (context.performed) {
            if (canFastFall) {
                Debug.Log("Fastfalling");
                rb.AddForce(new Vector2(0, -fastFallForce));
            }
        }
    }

    public void TouchGround() {
        coyoteTimer = 0;
        canFastFall = false;
    }

    /// <summary>
    /// If moving up, cancel coyote time
    /// </summary>
    public void LeaveGround() {
        if (rb.velocity.y > 0) {
            coyoteTimer = coyoteTime;
        }
    }
}
