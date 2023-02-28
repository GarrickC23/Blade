using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    // Start is called before the first frame update
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    private float currentDashTime = 0f;
    private bool isDashing = false;
    public Rigidbody2D rb;
    private Vector3 dashDirection;
    public PlayerControls playerControls;
    private InputAction dash;

    void Start()
    {
        playerControls = new PlayerControls();
        dash = playerControls.Ground.Dash;
        dash.Enable();
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
    private void Dash(InputAction.CallbackContext context)
    {
        if (context.action.name == "Dash" && !isDashing)
        {
            Debug.Log("can dash");
            isDashing = true;
            currentDashTime = dashDuration;
            Debug.Log(dashDirection);
            dashDirection.x = transform.forward.z;
            Debug.Log(dashDirection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            if (currentDashTime > 0)
            {

                rb.velocity = dashDirection * dashSpeed;
                currentDashTime -= Time.deltaTime;

            }
            else
            {
                isDashing = false;
            }
        }
    }
}
