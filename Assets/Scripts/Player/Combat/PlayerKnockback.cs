using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{

    public void PlayerKnockbackFunction(float strength, float angle, Transform XReferencePoint){
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
        
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir*Xdifference*strength*Mathf.Cos(Mathf.Deg2Rad*angle),strength*Mathf.Sin(Mathf.Deg2Rad*angle)), ForceMode2D.Impulse);
    }
}
