using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerKnockback;

public class EnemyAttack : MonoBehaviour
{
    public bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage, angle, knockbackPower, stunDuration;
    public Direction direction; 
    public float attackRange;
    public GameObject sparks;
    public float attackCooldown;
    private Animator anim;
    public string attackName;

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!isAttacking) StartCoroutine("AttackCoroutine");
        // if (isAttacking) Attack();
        if (!isAttacking && InRange()) {
            isAttacking = true;
            anim.Play(attackName);
        }
        if (isAttacking) {
            Attack();
        }
    }

    private void Attack()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);
        foreach (Collider2D hit in Hits)
        {
            Debug.Log("Collider hit " + hit.gameObject);
            if(hit.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
            {
                player.Attacked(damage, angle, knockbackPower, stunDuration, direction);

                GameObject spark = Instantiate(sparks, attackPosition.position, Quaternion.identity);
                spark.GetComponent<ParticleSystem>().Play(); 
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

    private bool InRange() {
        Vector2 currPos = this.gameObject.transform.position;
        Vector2 direction = (Vector2)(player.transform.position) - currPos;
        RaycastHit2D ray = Physics2D.Raycast(currPos, direction, attackRange, 3);
        if (ray && ray.collider.gameObject == player) {
            return true;
        }

        Debug.Log(ray.distance);
        return false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
