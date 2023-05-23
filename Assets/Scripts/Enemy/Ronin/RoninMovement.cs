using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninMovement : EnemyMovement1
{
    // public float posXPaceDistance;
    // public float negXPaceDistance;

    // public float walkSpeed;

    // public float aggroRange;

    // public float tetherRange; // max distance from enemy's starting position, left patrol pos, and right patrol pos

    // bool movingRight = true;
    // bool isPatrolling = true;
    
    GameObject player;

    protected override void EnemyWalk(){
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

    protected override bool InRange() {
        Vector2 currPos = gameObject.transform.position;
        Vector2 direction = (Vector2)(player.transform.position) - currPos;
        RaycastHit2D ray = Physics2D.Raycast(currPos, direction, aggroRange, 3);
        if (ray && (ray.collider.gameObject == player || ray.collider.transform.parent.gameObject == player)) {
            return true;
        }
        return false;
    }
    protected override void Chase(){
        if (player.transform.position.x > transform.position.x) { 
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, transform.GetComponent<Rigidbody2D>().velocity.y); //move to the right towards player
        }
        else {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-walkSpeed, transform.GetComponent<Rigidbody2D>().velocity.y); //move to the left towards player
        }
    }

    protected override bool IsWithinTetherRange(){
        if (transform.position.x >= posXPaceDistance + tetherRange || transform.position.x <= negXPaceDistance - tetherRange) {
            return false;
        }
        return true;
    }
    protected override void Patrol(){
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
}
