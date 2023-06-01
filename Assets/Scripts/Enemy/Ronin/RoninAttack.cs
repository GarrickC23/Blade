using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoninAttack : EnemyAttack
{
    private EnemyStats enemyStatsRef;

    private void Awake() {
        enemyStatsRef = GetComponent<EnemyStats>();
    }

    protected override void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (!enemyStatsRef.isAttacking && InAttackRange() && !enemyStatsRef.isStunned && !enemyStatsRef.isKnockedBack) {
            enemyStatsRef.isAttacking = true;
            anim.Play(attackName);
            PrevHits.Clear();
        }
        if (enemyStatsRef.isAttacking) {
            Attack();
        }
    }

    protected override void Attack()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);
        foreach (Collider2D hit in Hits)
        {
            if (PrevHits.Contains(hit)){
                continue;
            }
            PrevHits.Add(hit);
            if(hit.gameObject.TryGetComponent<PlayerCombat>(out PlayerCombat player))
            {   
                player.Attacked(damage, angle, knockbackPower, stunDuration, playerParryIncrease, transform, knockbackDuration);
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        enemyStatsRef.isAttacking = true;
        Attack();
        yield return new WaitForSeconds(1f);
        Attack();
        yield return new WaitForSeconds(1f);
        Attack();
        yield return new WaitForSeconds(1f);
        enemyStatsRef.isAttacking = false;
    }

    protected override bool InAttackRange() {
        Vector2 currPos = this.gameObject.transform.position;
        Vector2 direction = (Vector2)(player.transform.position) - currPos;
        RaycastHit2D ray = Physics2D.Raycast(currPos, direction, attackRange, 3);
        if (ray && (ray.collider.gameObject == player || (ray.collider.transform.parent != null && ray.collider.transform.parent.gameObject == player))) {
            return true;
        }

        // Debug.Log(ray.distance);
        return false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
