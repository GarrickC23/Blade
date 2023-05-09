using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator anim;

    public float parryDuration;
    public float delayBetweenBlocks;

    bool canBlock = true;

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
    

    //Parry Function. When you press "Q" at the right time before the attack hits you, you will parry the attack
    public void ParryAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !GetComponent<PlayerStats>().isGuarding && !GetComponent<PlayerStats>().isParrying && canBlock)
        {
            canBlock = false;
            GetComponent<PlayerStats>().isGuarding = true;
            StartCoroutine("ParryCoroutine");
            anim.SetBool("HeroKnight_Block", true); 
        }
    }

    //If you release "Q", then you will not be guarding anymore
    public void GuardRelease(InputAction.CallbackContext context)
    {
        if (context.canceled && GetComponent<PlayerStats>().isGuarding)
        {
            GetComponent<PlayerStats>().isGuarding = false;
            anim.SetBool("HeroKnight_Block", false);
            StopCoroutine("ParryCoroutine");
            GetComponent<PlayerStats>().isParrying = false;
            Invoke("RefreshBlock", delayBetweenBlocks);
        }
    }

    private IEnumerator ParryCoroutine()
    {
        GetComponent<PlayerStats>().isParrying = true;
        yield return new WaitForSeconds(parryDuration);
        GetComponent<PlayerStats>().isParrying = false;
    }

    void RefreshBlock(){
        canBlock = true;
    }
}