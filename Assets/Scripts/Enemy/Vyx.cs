using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerKnockback;

public class Vyx : Enemy
{
    private Animator anim;
    public GameObject player;
    public Transform playerPos;

    [Header("Attack")] 
    private bool isAttacking = false;
    public Transform attackPosition;
    public float attackRadius;
    public float damage, angle, knockbackPower, stunDuration;
    public Direction direction; 

    [Header("Stats")]
    [SerializeField] private float health = 10f; 

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private LayerMask detectWhat = 0;
    [SerializeField] private Transform sightTop = null;
    [SerializeField] private Transform sightBot = null;
    private float dir = -1; 

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        EnemyWalk();
        if (!isAttacking) StartCoroutine("AttackCoroutine");
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    protected override void EnemyWalk()
    {
        anim.SetBool("FoxWalk", true);
        if (Physics2D.Linecast(sightTop.position, sightBot.position, detectWhat))
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 3f * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(walkSpeed * dir * Time.deltaTime, rb.velocity.y);
        }
    }

    protected override void EnemyAttack()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);
        //Debug.Log(isFront());
        if ( isFront() )
        {
            anim.Play("FoxAttack1");
            foreach (Collider2D hit in Hits)
            {
                if(hit.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
                {
                    player.Attacked(damage, angle, knockbackPower, stunDuration, direction);
                }
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }

    bool isFront()
    {
        Vector3 directionOfPlayer = transform.position - playerPos.position; 
        float angle = Vector3.Angle(directionOfPlayer, transform.forward);
        if (Mathf.Abs(angle) == 90 && directionOfPlayer.x < 3)
        {
            Debug.DrawLine(transform.position, playerPos.position, Color.red);
            return true;
        }
        
        return false;
    }

    // protected override void Think()
    // {
    //     if (Physics2D.Linecast(sightTop.position, sightBot.position, detectWhat))
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 3f * Time.deltaTime);
    //     }
    // }
}
