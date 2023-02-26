using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage;
    // Update is called once per frame
    void Update()
    {
        if (!isAttacking) StartCoroutine("AttackCoroutine");
    }

    private void Attack(){
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);
            foreach (Collider2D hit in Hits)
            {
                //Debug.Log(hit.gameObject);
                if(hit.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player)){
                    if (!player.isParrying){
                        player.TakeDamage(damage);
                    }
                }
            }
    }

    private IEnumerator AttackCoroutine(){
        isAttacking = true;
        Attack();
        yield return new WaitForSeconds(0.25f);
        Attack();
        yield return new WaitForSeconds(0.25f);
        Attack();
        yield return new WaitForSeconds(5f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
