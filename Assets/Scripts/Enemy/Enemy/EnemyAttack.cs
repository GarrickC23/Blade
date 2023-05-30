using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyAttack : MonoBehaviour
{
    public bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage, angle, knockbackPower, stunDuration, knockbackDuration, playerParryIncrease;
    public float attackRange;
    public GameObject sparks;
    protected Animator anim;
    public string attackName;

    protected GameObject player;

    protected List<Collider2D> PrevHits = new List<Collider2D>();

    protected abstract void Start();

    protected abstract void Update();

    protected abstract void Attack();

    protected abstract bool InAttackRange();
}
