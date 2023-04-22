using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    private Movement mv;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mv = player.GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            mv.grapplePoints.Add(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            mv.grapplePoints.Remove(this.gameObject);
        }
    }
}
