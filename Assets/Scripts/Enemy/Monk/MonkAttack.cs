using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkAttack : EnemyAttack
{
    private EnemyStats enemyStatsRef;
    [SerializeField]
    private Projectile projectile;

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
            StartCoroutine(AttackCoroutine());
            anim.Play(attackName);
            PrevHits.Clear();
            Debug.Log("Attacking");
        }
    }

    protected override void Attack()
    {
        Projectile proj = Instantiate(projectile, attackPosition);
        Vector2 direction = player.transform.position - attackPosition.transform.position;
        proj.AddForce(direction, proj.speed);
    }

    private IEnumerator AttackCoroutine()
    {
        enemyStatsRef.isAttacking = true;
        Attack();
        yield return new WaitForSeconds(3f);
        // Attack();
        // yield return new WaitForSeconds(1f);
        // Attack();
        // yield return new WaitForSeconds(1f);
        enemyStatsRef.isAttacking = false;
    }

    protected override bool InAttackRange() {
        Vector2 currPos = this.gameObject.transform.position;
        Vector2 direction = (Vector2)(player.transform.position) - currPos;
        RaycastHit2D ray = Physics2D.Raycast(currPos, direction, attackRange, 3);
        // if (ray && (ray.collider.gameObject == player || (ray.collider.transform.parent != null && ray.collider.transform.parent.gameObject == player))) {
        if (ray) {
            return true;
        }

        // Debug.Log(ray.distance);
        return false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, attackRange);
        Gizmos.DrawLine(this.transform.position, player.transform.position);
    }
}
