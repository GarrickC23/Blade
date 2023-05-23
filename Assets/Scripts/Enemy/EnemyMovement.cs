using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Enemy
{

    public float posXPaceDistance;
    public float negXPaceDistance;

    public float walkSpeed;

    public float aggroRange;

    public float tetherRange; // max distance from enemy's starting position, left patrol pos, and right patrol pos

    bool movingRight = true;
    bool isPatrolling = true;
    
    GameObject player;

    EnemyStats enemyStatsRef;
    private void Awake() {
        enemyStatsRef = GetComponent<EnemyStats>();
    }
    protected override void Start() {
        posXPaceDistance = transform.position.x + posXPaceDistance;
        negXPaceDistance = transform.position.x - negXPaceDistance;
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        EnemyWalk();
        if ((enemyStatsRef.isAttacking || enemyStatsRef.isStunned) && !enemyStatsRef.isKnockedBack){
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
    }
    protected override void EnemyWalk(){
        if (enemyStatsRef.isStunned || enemyStatsRef.isKnockedBack){
            return;
        }
        if (InRange() && IsWithinTetherRange()){ //if the player is in range of the enemy and the enemy is within its tether range
            isPatrolling = false;
        }
        else if (!IsWithinTetherRange()){
            isPatrolling = true;
        }

        if (isPatrolling) {
            Patrol();
        }
        else{
            Chase();
        }
    }

    private bool InRange() {
        Vector2 currPos = gameObject.transform.position;
        Vector2 direction = (Vector2)(player.transform.position) - currPos;
        RaycastHit2D ray = Physics2D.Raycast(currPos, direction, aggroRange, 3);
        if (ray && (ray.collider.gameObject == player || ray.collider.transform.parent.gameObject == player)) {
            return true;
        }
        return false;
    }
    private void Chase(){
        if (player.transform.position.x > transform.position.x) { 
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, transform.GetComponent<Rigidbody2D>().velocity.y); //move to the right towards player
        }
        else {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-walkSpeed, transform.GetComponent<Rigidbody2D>().velocity.y); //move to the left towards player
        }
    }

    private bool IsWithinTetherRange(){
        if (transform.position.x >= posXPaceDistance + tetherRange || transform.position.x <= negXPaceDistance - tetherRange) {
            return false;
        }
        return true;
    }
    private void Patrol(){
        if (movingRight) {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, transform.GetComponent<Rigidbody2D>().velocity.y);
        }
        else {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-walkSpeed, transform.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (transform.position.x >= posXPaceDistance) {
            movingRight = false;
        } 
        else if (transform.position.x <= negXPaceDistance) {
            movingRight = true;
        }
    }

    protected override void EnemyAttack(){

    }

    public override void TakeDamage(float damage){

    }
}
