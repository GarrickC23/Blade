using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject sparks; 
    [SerializeField] private Parry parry;

    void Start() {
        parry = GameObject.FindGameObjectWithTag("Player").GetComponent<Parry>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if ( parry.isParrying == true )
        {
            Debug.Log("You parried");
            GameObject spark = Instantiate(sparks, transform.position, Quaternion.identity);
            spark.GetComponent<ParticleSystem>().Play(); 
        }
        else
        {
            Debug.Log("Unsuccessful parry");
        }
    }
}
