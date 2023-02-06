using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    void Start()
    {
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed){
            Debug.Log("swung");
        } 
    }
}
