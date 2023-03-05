using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public enum Direction {TOWARDS, AWAY};
    public void PlayerKnockbackFunction(float strength, float angle, Direction direction, Transform XReferencePoint, float stunDuration){
        
        StartCoroutine(StunCoroutine(strength, angle, direction, XReferencePoint, stunDuration));
    }

    private IEnumerator StunCoroutine(float strength, float angle, Direction direction, Transform XReferencePoint, float stunDuration)
    {
        int dir, Xdifference;
        
        if (direction == Direction.TOWARDS){
            dir = -1;
        }
        else dir = 1;

        if (XReferencePoint.position.x - transform.position.x >= 0)
        {
            Xdifference = -1;
        }
        else 
        Xdifference = 1;
        gameObject.GetComponent<Movement>().enabled = false;
        GetComponent<PlayerStats>().isStunned = true;
        print("is stunned");
        yield return new WaitForSeconds(stunDuration);
        GetComponent<PlayerStats>().isStunned = false;
        print("not stunned");
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir*Xdifference*strength*Mathf.Cos(Mathf.Deg2Rad*angle),strength*Mathf.Sin(Mathf.Deg2Rad*angle)), ForceMode2D.Impulse);
    }
}
