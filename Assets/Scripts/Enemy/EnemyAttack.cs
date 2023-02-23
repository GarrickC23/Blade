using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage;

    public GameObject hitBox;               //test GameObject hitBox

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
                //Debug.Log(hit.gameObject);
                if(hit.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
                {
                    if (!player.isParrying)
                    {
                        player.TakeDamage(damage);
                    }
                }
                Vector3 position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
                var temp = Instantiate(hitBox, position, transform.rotation);
                Destroy(temp, 0.25f);
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
        yield return new WaitForSeconds(3f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
