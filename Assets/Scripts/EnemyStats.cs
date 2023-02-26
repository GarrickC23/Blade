using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    float Health;

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
