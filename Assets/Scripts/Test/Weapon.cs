using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Parry parry; 

    void OnTriggerEnter2D(Collider2D other) 
    {
        if ( parry.isParrying == true )
        {
            Debug.Log("You parried the Matt Attack of the Century");
        }
        else
        {
            Debug.Log("You failed the Brent. NOONONOOOOO - Brent Voice");
        }
    }
}
