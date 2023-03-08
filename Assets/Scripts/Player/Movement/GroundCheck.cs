using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    private Movement movementScript;

    private void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            movementScript.TouchGround();
            transform.parent.GetComponent<PlayerStats>().isKnockedBack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            movementScript.LeaveGround();
        }
    }
}
