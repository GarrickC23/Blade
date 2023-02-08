using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerControls playerControls;
    public float attackRadius;
    public Transform attackPosition;
    private LayerMask enemyLayer = 3;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    void Start()
    {
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed){
            Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, enemyLayer);
            foreach (var hit in Hits)
            {
                Swing();
            }
        } 
    }

     private void Swing(){
        Debug.Log("swung");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
