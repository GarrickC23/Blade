using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static EnemyKnockback;

public class PlayerAttack : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator anim;

    public float attackRadius;
    public Transform attackPosition;

    public float swingAttackDamage, angle, knockbackPower, stunDuration;

    public Direction direction; 

    public float attackCoolDown;

    float attackTimer = 0f;

    bool canAttack = true;

    private void Awake() 
    {
        anim = gameObject.GetComponent<Animator>(); 
        playerControls = new PlayerControls();
    }

    private void OnEnable() 
    {
        playerControls.Enable();
    }

    private void OnDisable() 
    {
        playerControls.Disable();
    }

    private void Update() 
    {
        if (attackTimer > attackCoolDown && canAttack == false)
        {
            canAttack = true;
        }
        else attackTimer += Time.deltaTime;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !GetComponent<PlayerStats>().isKnockedBack && !GetComponent<PlayerStats>().isStunned)
        {
            //Debug.Log("Attack");
            anim.Play("HeroKnight_Attack2");
            attackTimer = 0;
            canAttack = false;
            Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D hit in Hits)
            {
                Swing(hit);
            }
        } 
    }

    private void Swing(Collider2D hit)
    {
        //Debug.Log(hit.gameObject);
        if(hit.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemy))
        {
            enemy.EnemyAttacked(swingAttackDamage, angle, knockbackPower, stunDuration, direction, transform);
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
