using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPotion : MonoBehaviour
{
    private PlayerControls playerControls;
    public PlayerStats stats; 

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Ground.Heal.performed += Heal;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable(); 
    }

    private void Heal(InputAction.CallbackContext context)
    {
        if ( context.performed )
        {
            if ( stats.health <= stats.maxHealth )
            {
                Debug.Log(stats.health);
                stats.health += 1;
            }
            stats.HeartUpdate();
        }
    }
}
