using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyAttack1 : MonoBehaviour
{
    public bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage, angle, knockbackPower, stunDuration, knockbackDuration, playerParryIncrease;
    public float attackRange;
    public GameObject sparks;
    private Animator anim;
    public string attackName;

    private GameObject player;

    private List<Collider2D> PrevHits = new List<Collider2D>();

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!isAttacking) StartCoroutine("AttackCoroutine");
        // if (isAttacking) Attack();
        if (!isAttacking && InRange() && !GetComponent<EnemyStats>().isStunned) {
            isAttacking = true;
            anim.Play(attackName);
            PrevHits.Clear();
        }
        if (isAttacking) {
            Attack();
        }
    }

    protected abstract void Attack();

    protected abstract bool InRange();
}
