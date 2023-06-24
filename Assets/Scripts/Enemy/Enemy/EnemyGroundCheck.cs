using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            isGrounded = false;
        }
    }
}
