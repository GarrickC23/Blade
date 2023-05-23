using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void EnemyKnockbackFunction(float strength, float angle, Transform XReferencePoint){
        int dir;
        
        if (XReferencePoint.position.x - transform.position.x >= 0){
            dir = -1;
        }
        else dir = 1;

        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir*strength*Mathf.Cos(Mathf.Deg2Rad*angle),strength*Mathf.Sin(Mathf.Deg2Rad*angle)), ForceMode2D.Impulse);
    }
}

