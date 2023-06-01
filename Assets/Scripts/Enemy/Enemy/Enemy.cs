using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerKnockback;

public abstract class Enemy : MonoBehaviour
{   
    protected Rigidbody2D rb;

    //public EnemyMovement enemyMovement; 

    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public abstract void TakeDamage(float damage);
    
    protected abstract void EnemyWalk();

    protected abstract void EnemyAttack();

    //protected abstract void Think();

    //TODO: Add Enemy Knockback
    // public EnemyKnockback enemyKnockback

    //TODO: Enemy Attack
    // public EnemyAttack enemyAttack    call EnemyAttack script
}
