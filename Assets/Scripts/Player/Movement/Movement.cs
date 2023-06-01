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
    private float jumpForce;                            // How fast the player moves upwards initially
    private float initialVelocity;                      // Same as jumpForce but different formula (testing)
    public float jumpHeight;                            // How high the player jumps
    public float jumpTime;                              // How long it takes the player to reach the height of their jump
    public float downwardsForce;                        // How much the player slows down at apex of jump
    public float fastFallForce;
    public float coyoteTime;                            // Time window player is allowed to execute an action
    private float coyoteTimer;                          // Base timer set to 0.
    private bool canFastFall;
    public bool isGrounded;

    [Header("WallJump")]
    [SerializeField]
    private WallJumpCheck wallJumpCheck;
    [HideInInspector] public bool canWallJump;          // Checks to see if the player is within a wall
    public float horizontalWallJumpForce;               // How far off the wall player is sent
    public float wallJumpFrozenTime;                    // How long the player can't move after wall jumping
    private GameObject lastTouchedWall;                 // Wall that was last jumped off of, gets set to null after touching ground

    [Header("Running")]
    public float minSpeed;                  
    public float maxSpeed;                              // Max speed player can move
    public float smoothTime;                            //Approximately the time it will t ake to reach the target. Smaller value will reach the target faster.
    private Vector2 m_Velocity = Vector2.zero;          
    private float velocityTolerance = 1f;               

    [Header("Dashing")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingDir; 
    public float dashingFrozenTime; 
    [SerializeField] private float dashingPower = 24f; 
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f; 

    [Header("Grapples")]
    public List<GameObject> grapplePoints;
    public float grappleSpeed;

    private PlayerControls playerControls;              //Input System variable
    private float freezeTimer;                          //Freeze movement
    [SerializeField] private TrailRenderer tr; 
    [SerializeField] private LockOn lockOn;

    public enum PlayerStates { Idle, Walk, Jump };
    public PlayerStates playerState; 
    
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>(); 
        grapplePoints = new List<GameObject>();
        playerControls = new PlayerControls();
        playerControls.Ground.Jump.performed += Jump; 
        playerControls.Ground.Jump.canceled += JumpRelease;
        playerControls.Ground.FastFall.performed += FastFall;
        playerControls.Ground.Dash.performed += Dash; 
        playerControls.Ground.Grapple.performed += Grapple;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void onDisable()
    {
        playerControls.Disable(); 
    }

    void Start()
    {
        canFastFall = false;
        canWallJump = false;
        isGrounded = false;

        initialVelocity = (2 * jumpHeight) / jumpTime;                              //Initial velocity formula  
        float gravityStrength = -(2 * jumpHeight) / (jumpTime * jumpTime);          //Gravity formula
        float gravityScale = gravityStrength / Physics2D.gravity.y;
        rb.gravityScale = gravityScale;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTime;                          //Same as initial velocity (different formula)

        coyoteTimer = 0;
        freezeTimer = 0;
    }
    
    private void Update() 
    {
        Move();
        Flip();
        // Debug.Log("direction" + GetDirection());

        getAnimState(playerState);                  //Get animation state enum
        
        //Brief time for players to have extra time to jump or any other action. (Timer)
        if (coyoteTimer < coyoteTime && !isGrounded) 
        {
            coyoteTimer += Time.deltaTime;
        }

        //Briefly stops player from moving. (Timer)
        if (freezeTimer > 0) 
        {
            freezeTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            return;
        }
    }

    private void Move() 
    {
        if (GetComponent<PlayerStats>().isStunned || GetComponent<PlayerStats>().isKnockedBack || freezeTimer > 0) return; // //hotfix so that move does not interfere with stun.

        //Gets the WASD/Arrow Keys from Input System as a Vector2 (x, y)
        Vector2 move = playerControls.Ground.Movement.ReadValue<Vector2>();

        //Moves character left and right.
        // Set animations and rotation
        if (!isGrounded)
        {
            playerState = PlayerStates.Jump;

            if (rb.velocity.y < 0) {
                anim.Play("HeroKnight_Fall");            
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > velocityTolerance)
        {
            playerState = PlayerStates.Walk;
        }
        //Check to see if player is not moving to play idle animation
        else
        {
            // moveSpeed -= decceleration * Time.deltaTime; 
            playerState = PlayerStates.Idle;
        }
        
        //Acceleration and Deceleration for player. 
        //SmoothDamp(current position, target position, current velocity, time to reach to target)
        Vector2 targetVelocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
        Vector2 m_Velocity2 = new Vector2(0, rb.velocity.y);

        // If the player is in the air and isn't pressing any keys, keep momentum
        if (isGrounded || targetVelocity.x != 0) {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity2, smoothTime);
        }
    }

    /// <summary>
    /// Jump Function. If player is within jump window, jump. Else if player is attatched to wall, wall jump. 
    /// </summary>
    /// <returns></returns>
    private void Jump(InputAction.CallbackContext context)
    {

        //Debug.Log(context);
        if (context.performed && freezeTimer <= 0)
        {
            //If player is within jump window, jump. Else if player is attatched to wall, wall jump. 
            if (coyoteTimer < coyoteTime) 
            {
                playerState = PlayerStates.Jump;
                StartCoroutine(Jump());
            }
            else if (canWallJump && wallJumpCheck.attachedWall != lastTouchedWall) 
            {
                playerState = PlayerStates.Jump;
                StartCoroutine(WallJump());
            }
        }
    }

    /// <summary>
    //Jump Coroutine. Add force of initialVelocity, wait jumpTime seconds until player falls back down.
    //Use AddForce when falling down adding a downwardsForce value 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Jump()
    {
        coyoteTimer = coyoteTime;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.AddForce(new Vector2(0, initialVelocity), ForceMode2D.Impulse);

        yield return new WaitForSeconds(jumpTime);
        
        canFastFall = true;
        rb.AddForce(new Vector2(0, -downwardsForce));
        // anim.Play("HeroKnight_Fall");
    }

    /// <summary>
    /// Wall Jump Coroutine. Applies horizontal force to move player right or left. 
    /// Freeze Movement function to stop player from moving during wall jump. Wait until jumpTime. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WallJump()
    {
        lastTouchedWall = wallJumpCheck.attachedWall;
        int direction = -GetDirection();    // Move the player away from the wall
        if (direction == -1) {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.AddForce(new Vector2(direction * horizontalWallJumpForce, jumpForce), ForceMode2D.Impulse);
        // rb.velocity = new Vector2(direction * horizontalWallJumpForce, rb.velocity.y);

        FreezeMovement(wallJumpFrozenTime);
        yield return new WaitForSeconds(jumpTime);
        canFastFall = true;
    }

    private void Grapple(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Grappling");
            if (grapplePoints.Count > 0 && freezeTimer <= 0)
            {
                // Get closest grapple point
                GameObject closestPoint = grapplePoints[0];
                float minDist = Vector3.Distance(this.gameObject.transform.position, closestPoint.transform.position);
                for (int i=1; i < grapplePoints.Count; i++)
                {
                    float nextDist = Vector3.Distance(this.gameObject.transform.position, grapplePoints[i].transform.position);
                    if (nextDist < minDist)
                    {
                        closestPoint = grapplePoints[i];
                        minDist = nextDist;
                    }
                }

                // Grapple towards closest point
                StartCoroutine(GrappleTowardsObject(closestPoint, minDist));
            }
        }
    }

    private IEnumerator GrappleTowardsObject(GameObject ob, float minDist) {
        FreezeMovement(minDist/grappleSpeed/2);
        Vector3 direction = ob.transform.position - this.gameObject.transform.position;
        direction = Vector3.Normalize(direction);
        rb.velocity = direction * grappleSpeed;
        float oldGrav = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(minDist/grappleSpeed/2);
        rb.gravityScale = oldGrav;
    }

    /// <summary>
    /// If button is released, if player in air, y velocity set to 0 to fall down.
    /// </summary>
    /// <returns></returns>
    private void JumpRelease(InputAction.CallbackContext context)
    {
        if ( context.canceled )
        {
            if (rb.velocity.y > 0) 
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                anim.Play("HeroKnight_Fall");
            }
        }
    }

    //FastFall function
    private void FastFall(InputAction.CallbackContext context) 
    {
        if (context.performed && freezeTimer <= 0) 
        {
            if (canFastFall) 
            {
                rb.AddForce(new Vector2(0, -fastFallForce));
            }
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        int direction;// = GetDirection();

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; 

        if (rb.velocity.x < 0) 
        {
            direction = -1;
        }
        else 
        {
            direction = 1;
        }

        rb.velocity = Vector3.zero;
        
        if (direction == 1)
        {
            rb.AddForce(Vector3.right * dashingPower, ForceMode2D.Impulse); 
        }
        else if (direction == -1)
        {
            rb.AddForce(Vector3.right * -dashingPower, ForceMode2D.Impulse);
        }

        tr.emitting = true;
        FreezeMovement(dashingFrozenTime);
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    /// <summary>
    /// Returns the direction the player is facing
    /// 1 for right, -1 for left
    /// </summary>
    /// <returns></returns>
    private int GetDirection()
    {
        if (this.transform.rotation.y < 0) 
        {
            return -1;
        }
        else 
        {
            return 1;
        }
    }

    private void Flip()
    {
        if (!lockOn.IsLocked()) {
            if (rb.velocity.x > velocityTolerance /*&& moveSpeed < maxSpeed */)
            {
                // moveSpeed += acceleration * Time.deltaTime; 
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (rb.velocity.x < -velocityTolerance /*&& moveSpeed < maxSpeed */)
            {
                // moveSpeed += acceleration * Time.deltaTime; 
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else {
            lockOn.TurnTowardsEnemy();
        }
    }

    /// <summary>
    /// When the player touches ground, reset the jump and wall jump
    /// </summary>
    public void TouchGround() 
    {
        coyoteTimer = 0;
        canFastFall = false;
        lastTouchedWall = null;
        isGrounded = true;

        // playerState = PlayerStates.Idle;
    }

    /// <summary>
    /// If moving up, cancel coyote time
    /// </summary>
    public void LeaveGround() 
    {
        if (rb.velocity.y > 0) 
        {
            coyoteTimer = coyoteTime;
        }
        isGrounded = false;
    }

    /// <summary>
    /// Prevents the player from moving for a certain amount of time
    /// </summary>
    /// <param name="frozenTime"></param>
    public void FreezeMovement(float frozenTime)
    {
        freezeTimer = frozenTime;
    }

    /// <summary>
    /// Animation State Machine
    /// </summary>
    public void getAnimState(PlayerStates playerState)
    {
        switch (playerState)
        {
            case PlayerStates.Idle:
                anim.SetBool("Idle", true);
                anim.SetBool("Run", false);
                anim.SetBool("HeroKnight_Jump", false);
                break;
            case PlayerStates.Walk:
                anim.SetBool("Run", true);
                anim.SetBool("Idle", false);
                anim.SetBool("HeroKnight_Jump", false);
                break;
            case PlayerStates.Jump:
                anim.SetBool("HeroKnight_Jump", true);
                anim.SetBool("Idle", false);
                anim.SetBool("Run", false);
                break;
            default:
                break;
        }
    }
}
