using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator anim;

    public float parryDuration;
    public bool isParrying = false;
    public bool isGuarding = false; 

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        playerControls = new PlayerControls();
        //playerControls.Ground.Parry.performed += ParryAttack;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void onDisable()
    {
        playerControls.Disable(); 
    }
    

    //Parry Function. When you press "Q" and let go at the right time, you will parry
    public void ParryAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isParrying && !isGuarding)
        {
            isGuarding = true;
            GetComponent<PlayerStats>().isGuarding = true;

            if ( isGuarding )
            {
                anim.SetBool("HeroKnight_Block", true);
            }
            
            StartCoroutine("ParryCoroutine");
        }
    }

    //If you release "Q", then you will not be guarding anymore
    public void GuardRelease(InputAction.CallbackContext context)
    {
        if (context.canceled && isGuarding)
        {
            isGuarding = false;
            GetComponent<PlayerStats>().isGuarding = false;
            anim.SetBool("HeroKnight_Block", false);
        }
    }

    private IEnumerator ParryCoroutine()
    {
        isParrying = true;
        GetComponent<PlayerStats>().isParrying = true;
        yield return new WaitForSeconds(parryDuration);
        GetComponent<PlayerStats>().isParrying = false;
        
        //If you want to add a delay between each parry add another wait for seconds here

        isParrying = false;
    }
}