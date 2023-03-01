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
            rb.velocity = new Vector2(dashSpeed * transform.right.x, 0);

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
        if (context.performed && currentDashCooldown <= 0f)
        {
            Debug.Log("Dash!");
            isDashing = true;
            currentDashTime = dashTime;
        }
    }
}


