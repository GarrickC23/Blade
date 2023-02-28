using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpCheck : MonoBehaviour
{
    [SerializeField]
    private Movement movementScript;

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            movementScript.canWallJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            movementScript.canWallJump = false;
        }
    }
}
