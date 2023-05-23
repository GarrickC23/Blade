using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement1 : MonoBehaviour
{

    public float posXPaceDistance;
    public float negXPaceDistance;

    public float walkSpeed;

    public float aggroRange;

    public float tetherRange; // max distance from enemy's starting position, left patrol pos, and right patrol pos

    public bool movingRight = true;
    public bool isPatrolling = true;
    
    GameObject player;

    protected void Start() {
        posXPaceDistance = transform.position.x + posXPaceDistance;
        negXPaceDistance = transform.position.x - negXPaceDistance;
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        EnemyWalk();
        if (GetComponent<EnemyStats>().isAttacking){
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    protected abstract void EnemyWalk();

    protected abstract bool InRange();

    protected abstract void Chase();

    protected abstract bool IsWithinTetherRange();

    protected abstract void Patrol();
}
