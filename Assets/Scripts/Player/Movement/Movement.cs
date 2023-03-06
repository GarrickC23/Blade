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
    private bool canFastFall;

    [Header("WallJump")]
    [SerializeField]
    private WallJumpCheck wallJumpCheck;
    [HideInInspector] public bool canWallJump;  // Checks to see if the player is within a wall
    public float horizontalWallJumpForce;       // How far off the wall player is sent
    public float wallJumpFrozenTime;                    // How long the player can't move after wall jumping
    private GameObject lastTouchedWall;         // Wall that was last jumped off of, gets set to null after touching ground

    [Header("Running")]
    public float acceleration;              // How fast the player accelerates starting from moveSpeed
    public float decceleration;             // How fast the player decelerates when user is not inputting any button
    public float minSpeed;                  
    public float maxSpeed;                  // Max speed player can move
    private Vector2 m_Velocity = Vector2.zero;
    private float velocityTolerance = 1f;

    private PlayerControls playerControls;
    private float freezeTimer;
    
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
        canWallJump = false;
        float gravityStrength = -(2 * jumpHeight) / (jumpTime * jumpTime);
        float gravityScale = gravityStrength / Physics2D.gravity.y;
        rb.gravityScale = gravityScale;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTime;
        coyoteTimer = 0;
        freezeTimer = 0;
    }
    
    private void Update() {
        Move();
        if (coyoteTimer < coyoteTime) {
            coyoteTimer += Time.deltaTime;
        }
        if (freezeTimer > 0) {
            freezeTimer -= Time.deltaTime;
        }
    }

    private void Move() {
        if ( rb.velocity.x > velocityTolerance /*&& moveSpeed < maxSpeed */)
        {
            // moveSpeed += acceleration * Time.deltaTime; 
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }
        else if ( rb.velocity.x < -velocityTolerance /*&& moveSpeed < maxSpeed */)
        {
            // moveSpeed += acceleration * Time.deltaTime; 
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }
        else if ( moveSpeed > maxSpeed )
        {
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }

        if (GetComponent<PlayerStats>().isStunned || freezeTimer > 0) return; // //hotfix so that move does not interfere with stun.
        //Gets the WASD/Arrow Keys from Input System as a Vector2 (x, y)
        Vector2 move = playerControls.Ground.Movement.ReadValue<Vector2>();
        // Debug.Log(move);
        Vector2 targetVelocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
        Vector2 m_Velocity2 = new Vector2(0, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity2, 0.05f);


        if ( move.x == 0 && moveSpeed > minSpeed )
        {
            // moveSpeed -= decceleration * Time.deltaTime; 
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
        }
    }

    //Jump Function. When you press "Space" you will jump
    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if ( context.performed  && freezeTimer <= 0)
        {
            if (coyoteTimer < coyoteTime) {
                Debug.Log("Jump!"); //Implement Jump 
                inAir = true;

                anim.SetBool("Idle", false);
                anim.SetBool("Run", false);
                anim.SetBool("HeroKnight_Jump", true);

                StartCoroutine(Jump());
            }
            else if (canWallJump && wallJumpCheck.attachedWall != lastTouchedWall) {
                Debug.Log("Wall Jump");
                inAir = true;

                anim.SetBool("Idle", false);
                anim.SetBool("Run", false);
                anim.SetBool("HeroKnight_Jump", true);
                StartCoroutine(WallJump());
            }
        }
    }

    private IEnumerator Jump()
    {
        coyoteTimer = coyoteTime;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        yield return new WaitForSeconds(jumpTime);
        
        canFastFall = true;
        rb.AddForce(new Vector2(0, -downwardsForce));
        anim.Play("HeroKnight_Fall");
    }

    private IEnumerator WallJump()
    {
        lastTouchedWall = wallJumpCheck.attachedWall;
        int direction = -GetDirection();    // Move the player away from the wall
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.AddForce(new Vector2(direction * horizontalWallJumpForce, jumpForce), ForceMode2D.Impulse);
        rb.velocity = new Vector2(direction * horizontalWallJumpForce, rb.velocity.y);
        FreezeMovement(wallJumpFrozenTime);
        yield return new WaitForSeconds(jumpTime);
        canFastFall = true;

        // rb.AddForce(new Vector2(0, -downwardsForce));
        // anim.Play("HeroKnight_Fall");
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
        if (context.performed && freezeTimer <= 0) {
            if (canFastFall) {
                Debug.Log("Fastfalling");
                rb.AddForce(new Vector2(0, -fastFallForce));
            }
        }
    }

    /// <summary>
    /// Returns the direction the player is facing
    /// 1 for right, -1 for left
    /// </summary>
    /// <returns></returns>
    private int GetDirection()
    {
        if (this.transform.rotation.y < 0) {
            return -1;
        }
        else {
            return 1;
        }
    }

    /// <summary>
    /// When the player touches ground, reset the jump and wall jump
    /// </summary>
    public void TouchGround() {
        coyoteTimer = 0;
        canFastFall = false;
        lastTouchedWall = null;

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

    /// <summary>
    /// Prevents the player from moving for a certain amount of time
    /// </summary>
    /// <param name="frozenTime"></param>
    public void FreezeMovement(float frozenTime)
    {
        freezeTimer = frozenTime;
    }
}
