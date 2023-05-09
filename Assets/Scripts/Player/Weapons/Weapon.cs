using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject sparks; 

    void OnTriggerEnter2D(Collider2D other) 
    {
        if ( GetComponent<PlayerStats>().isParrying)
        {
            Debug.Log("You parried");
            GameObject spark = Instantiate(sparks, transform.position, Quaternion.identity);
            spark.GetComponent<ParticleSystem>().Play(); 
        }
        else if (GetComponent<PlayerStats>().isGuarding)
        {
            Debug.Log("You blocked");
            GameObject spark = Instantiate(sparks, transform.position, Quaternion.identity);
            spark.GetComponent<ParticleSystem>().Play(); 
        }
        {
            Debug.Log("Unsuccessful parry/block");
        }
    }
}
