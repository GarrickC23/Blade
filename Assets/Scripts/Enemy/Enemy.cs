using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerKnockback;

public abstract class Enemy : MonoBehaviour
{   
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public abstract void TakeDamage(float damage);
    
    protected abstract void EnemyWalk();

    protected abstract void EnemyAttack();

    //protected abstract void Think();
}
