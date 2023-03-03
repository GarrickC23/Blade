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
    private bool inAir;                     // Check if player is in the air

    [Header("Running")]
    public float acceleration;              // How fast the player accelerates starting from moveSpeed
    public float decceleration;             // How fast the player decelerates when user is not inputting any button
    public float minSpeed;                  
    public float maxSpeed;                  // Max speed player can move

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
        coyoteTimer = 0;
    }
    
    private void Update() {
        Move();
        if (coyoteTimer < coyoteTime) {
            coyoteTimer += Time.deltaTime;
        }
    }

    private void Move() {

        if (GetComponent<PlayerStats>().isStunned) return; // //hotfix so that move does not interfere with stun.
        //Gets the WASD/Arrow Keys from Input System as a Vector2 (x, y)
        Vector2 move = playerControls.Ground.Movement.ReadValue<Vector2>();
        // Debug.Log(move);
        rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
        if ( rb.velocity.x > 0 && moveSpeed < maxSpeed )
        {
            moveSpeed += acceleration * Time.deltaTime; 
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }
        else if ( rb.velocity.x < 0 && moveSpeed < maxSpeed )
        {
            moveSpeed += acceleration * Time.deltaTime; 
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }
        else if ( moveSpeed > maxSpeed )
        {
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }

        if ( move.x == 0 && moveSpeed > minSpeed )
        {
            moveSpeed -= decceleration * Time.deltaTime; 
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
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
                inAir = true;

                anim.SetBool("Idle", false);
                anim.SetBool("Run", false);
                anim.SetBool("HeroKnight_Jump", true);

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
        anim.Play("HeroKnight_Fall");
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

        anim.SetBool("Idle", true);
        anim.SetBool("HeroKnight_Jump", false);
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
