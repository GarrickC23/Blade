using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerControls playerControls;
    public float attackRadius;
    public Transform attackPosition;

    public float swingAttackDamage;

    public float attackCoolDown;

    float attackTimer = 0f;

    bool canAttack = true;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    private void Update() {
        if (attackTimer > attackCoolDown && canAttack == false){
            canAttack = true;
        }
        else attackTimer += Time.deltaTime;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack){
            attackTimer = 0;
            canAttack = false;
            Debug.Log("Swing");
            Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D hit in Hits)
            {
                Swing(hit);
            }
        } 
    }

     private void Swing(Collider2D hit){
        Debug.Log(hit.gameObject);
        if(hit.gameObject.TryGetComponent<Enemy>(out Enemy enemy)){
            enemy.TakeDamage(swingAttackDamage);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
