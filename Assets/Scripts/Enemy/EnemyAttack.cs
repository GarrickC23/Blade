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

    public GameObject hitBox;               //test GameObject hitBox

    void Start()
    {
        anim = hitBox.GetComponent<Animator>(); 
    }

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
                    player.Attacked(damage, angle, knockbackPower, stunDuration, direction);
                }
                Vector3 position = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
                anim.Play("DummyAttack");
                var temp = Instantiate(hitBox, position, transform.rotation);
                Destroy(temp, 1f);
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
