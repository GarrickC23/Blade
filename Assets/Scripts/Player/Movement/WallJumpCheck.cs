using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpCheck : MonoBehaviour
{
    [SerializeField]
    private Movement movementScript;

    public GameObject attachedWall { get; private set; }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            movementScript.canWallJump = true;
            attachedWall = coll.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            movementScript.canWallJump = false;
            attachedWall = null;
        }
    }
}
