using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private PlayerControls playerControls;

    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;

    private bool isDashing = false;
    private bool isGrounded = true; // assume player starts on the ground
    private float currentDashTime = 0f;
    private float currentDashCooldown = 0f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Ground.Dash.performed += Dash;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if (isDashing)
        {
            // Move horizontally at a constant speed during the dash
           
            currentDashTime -= Time.deltaTime;
            if (currentDashTime <= 0f)
            {
                isDashing = false;
                currentDashCooldown = dashCooldown;
            }
        }
        else if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.deltaTime;
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        
        if (context.performed && !isDashing && currentDashCooldown <= 0f)
        {
            Debug.Log("Dash!");
            isDashing = true;
            currentDashTime = dashTime;
            Debug.Log(currentDashTime);
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        float originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        while (currentDashTime > 0f)
        {
            yield return null;
            rb.velocity = new Vector2(dashSpeed * rb.velocity.x,rb.velocity.y);
            currentDashTime -= Time.deltaTime;
        }
        rb.gravityScale = originalGravityScale;
    }
}
