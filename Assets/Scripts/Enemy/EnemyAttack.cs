using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerKnockback;

public class EnemyAttack : MonoBehaviour
{
    private bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage, angle, knockbackPower, stunDuration;
    public Direction direction; 
    private Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking) StartCoroutine("AttackCoroutine");
    }

    private void Attack()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);
        foreach (Collider2D hit in Hits)
        {
            if(hit.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
            {
                player.Attacked(damage, angle, knockbackPower, stunDuration, direction);
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        Attack();
        yield return new WaitForSeconds(1f);
        Attack();
        yield return new WaitForSeconds(1f);
        Attack();
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
