using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void PlayerKnockbackFunction(float strength, float angle, Transform XReferencePoint, float stunDuration, float knockbackDuration){
        
        StartCoroutine(StunCoroutine(strength, angle, XReferencePoint, stunDuration, knockbackDuration));
    }

    private IEnumerator StunCoroutine(float strength, float angle, Transform XReferencePoint, float stunDuration, float knockbackDuration)
    {
        int dir, Xdifference;
        
        if (XReferencePoint.position.x - transform.position.x >= 0){
            dir = 1;
        }
        else dir = -1;

        if (XReferencePoint.position.x - transform.position.x >= 0)
        {
            Xdifference = -1;
        }
        else 
        Xdifference = 1;
        GetComponent<PlayerStats>().isStunned = true;
        anim.Play("HeroKnight_Hurt");
        yield return new WaitForSeconds(stunDuration);
        GetComponent<PlayerStats>().isStunned = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir*Xdifference*strength*Mathf.Cos(Mathf.Deg2Rad*angle),strength*Mathf.Sin(Mathf.Deg2Rad*angle)), ForceMode2D.Impulse);
        GetComponent<PlayerStats>().isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDuration);
        GetComponent<PlayerStats>().isKnockedBack = false;
    }
}
