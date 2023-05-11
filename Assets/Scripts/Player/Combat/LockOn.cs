using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockOn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    public float lockRadius;                            // How far the player can lock onto an enemy
    private PlayerControls playerControls;              // Input System variable

    void Awake()
    {
        playerControls = new PlayerControls(); 
        playerControls.Ground.LockOn.performed += Lock;  
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable(); 
    }


    public bool IsLocked() {
        return enemy != null;
    }

    public void Lock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 currPosition = this.gameObject.transform.position;
            GameObject closestEnemy = null;
            float closestDistance = float.PositiveInfinity;
            Collider2D[] Hits = Physics2D.OverlapCircleAll(this.transform.position, lockRadius);
            foreach (Collider2D hit in Hits) {
                if (hit.gameObject.tag == "Enemy") {
                    float distance = Vector3.Distance(currPosition, hit.gameObject.transform.position);
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemy = hit.gameObject;
                    }
                }
            }

            if (enemy != null && closestEnemy == enemy) {
                enemy = null;
            }
            else {
                enemy = closestEnemy;
            }
        }
    }

    public void TurnTowardsEnemy() {
        if (enemy == null) {
            return;
        }

        Vector3 direction = this.gameObject.transform.position - enemy.transform.position;
        if (direction.x > 0) {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
