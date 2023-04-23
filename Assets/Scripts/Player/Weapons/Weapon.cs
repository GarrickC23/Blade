using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Parry parry;

    void Start() {
        parry = GameObject.FindGameObjectWithTag("Player").GetComponent<Parry>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if ( parry.isParrying == true )
        {
            Debug.Log("You parried");
        }
        else
        {
            Debug.Log("Unsuccessful parry");
        }
    }
}
