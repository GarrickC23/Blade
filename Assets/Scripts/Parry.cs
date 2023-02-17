using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{
    private PlayerControls playerControls;

    public float parryDuration;

    private bool isParrying = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        //playerControls.Ground.Parry.performed += ParryAttack;
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void onDisable(){
        playerControls.Disable(); 
    }
        //Parry Function. When you press "R" you will parry
    public void ParryAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isParrying)
        {
            StartCoroutine("ParryCoroutine");
        }
    }

    private IEnumerator ParryCoroutine(){
        isParrying = true;
        GetComponent<PlayerStats>().isParrying = true;
        yield return new WaitForSeconds(parryDuration);
        GetComponent<PlayerStats>().isParrying = false;
        
        //if you want to add a delay between each parry add another wait for seconds here

        isParrying = false;
    }
}