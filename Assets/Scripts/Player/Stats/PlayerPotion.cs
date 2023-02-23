using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPotion : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerStats stats; 
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Ground.Heal.performed += Heal;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void onDisable()
    {
        playerControls.Disable(); 
    }

    private void Heal(InputAction.CallbackContext context)
    {
        if ( context.performed )
        {
            Debug.Log("Heal");
            if ( stats.Health <= stats.maxHealth )
            {
                stats.Health += 1; 
            }
        }
    }
}
