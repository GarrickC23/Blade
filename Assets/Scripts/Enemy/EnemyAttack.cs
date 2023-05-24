using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttack : MonoBehaviour
{
    public Transform attackPosition;
    public float attackRadius;
    public float damage, angle, knockbackPower, stunDuration, knockbackDuration, playerParryIncrease;
    public float attackRange;
    public GameObject sparks;
    private Animator anim;
    public string attackName;

    private GameObject player;

    private List<Collider2D> PrevHits = new List<Collider2D>();

    private EnemyStats enemyStatsRef;

    private void Awake() {
        enemyStatsRef = GetComponent<EnemyStats>();
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!isAttacking) StartCoroutine("AttackCoroutine");
        // if (isAttacking) Attack();
        if (!enemyStatsRef.isAttacking && InAttackRange() && !enemyStatsRef.isStunned && !enemyStatsRef.isKnockedBack) {
            enemyStatsRef.isAttacking = true;
            anim.Play(attackName);
            PrevHits.Clear();
        }
        if (enemyStatsRef.isAttacking) {
            Attack();
        }
    }

    private void Attack()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);
        foreach (Collider2D hit in Hits)
        {
            if (PrevHits.Contains(hit)){
                continue;
            }
            PrevHits.Add(hit);
            //Debug.Log("Collider hit " + hit.gameObject);
            if(hit.gameObject.TryGetComponent<PlayerCombat>(out PlayerCombat player))
            {   
                player.Attacked(damage, angle, knockbackPower, stunDuration, playerParryIncrease, transform, knockbackDuration);

                // GameObject spark = Instantiate(sparks, attackPosition.position, Quaternion.identity);
                // spark.GetComponent<ParticleSystem>().Play(); 
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

    private bool InAttackRange() {
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
