using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float Health;
    [HideInInspector]
     public bool isParrying = false;

     private void Start() {
        Health = maxHealth;
     }

     public void TakeDamage(float damage){
        Health -= damage;

        if (Health <= 0){
            Destroy(gameObject);
        }
     }
}
