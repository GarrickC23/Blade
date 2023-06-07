using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninMovement : EnemyMovement
{

    [Header("Patrol Points")]
    public float maxXPaceDistance; //max distance the enemy can pace to the right
    public float minXPaceDistance; //max distance the enemy can pace to the left

    private float currMaxXPaceDistance; //current max distance the enemy can pace to the right (changes to a random value within range when reaching currMaxXPaceDistance)
    private float currMinXPaceDistance; //current max distance the enemy can pace to the left (changes to a random value within range when reaching currMinXPaceDistance)

    [Header("Movement Speed")]
    //public float walkSpeed;

    public float aggroMoveSpeedMultiplier; //how much faster the enemy moves when chasing the player 1.5 = 50% faster

    [Header("Aggro/Tether Range")]

    //public float aggroRange;

    //public float tetherRange; // max distance from enemy's starting position, left patrol pos, and right patrol pos

    [Header("Timers/Durations")]
    public float tetherResetDuration; //how long the enemy has to be in tether range before it can chase the player again
    public float paceEndpointPauseDuration; //how long the enemy has to pause after reaching a patrol endpoint.

    //bool movingRight = true;
    //bool isPatrolling = true;
    
    public bool canMovePatrolling = true;
    bool canChase = true; //if the enemy can chase the player. Resets when the enemy is pulled out of tether range
    
    private Animator anim; 
    //private GameObject player;

    EnemyStats enemyStatsRef;
    private void Awake() {
        enemyStatsRef = GetComponent<EnemyStats>();
    }

    protected override void Start() {
        maxXPaceDistance = transform.position.x + maxXPaceDistance;
        minXPaceDistance = transform.position.x - minXPaceDistance;
        player = GameObject.FindWithTag("Player");

        anim = gameObject.GetComponent<Animator>();
    }

    protected override void Update() {
        if ((enemyStatsRef.isAttacking || enemyStatsRef.isStunned || !canMovePatrolling) && !enemyStatsRef.isKnockedBack){
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
        else{
            EnemyWalk();
        }
    }
    
    void LateUpdate(){
        if(GetComponent<Rigidbody2D>().velocity.x > 0){ //if the enemy is moving right
            transform.rotation = Quaternion.Euler(0, 180, 0); //make the enemy face right
        }
        else{
            transform.rotation = Quaternion.Euler(0, 0, 0); //make the enemy face left
        }
    }

    protected override void EnemyWalk(){
        anim.Play("Walk");
        if (enemyStatsRef.isStunned || enemyStatsRef.isKnockedBack){
            return;
        }
        if (InAggroRange() && IsWithinTetherRange()){ //if the player is in aggro range of the enemy and the enemy is within its tether range
            isPatrolling = false;
        }
        else if (!InAggroRange() && IsWithinTetherRange()){ // if the player is not within aggro range of the enemy and the enemy is within tether range
            isPatrolling = true;
        }
        else if (!IsWithinTetherRange()){
            isPatrolling = true;
            canChase = false;
            Invoke("CanChaseInvokeHelper", tetherResetDuration);
        }

        if (isPatrolling) {
            Patrol();
        }
        else{
            Chase();
        }
    }

    protected override bool InAggroRange() {
        anim.SetBool("Fight", true);
        Vector2 currPos = gameObject.transform.position;
        Vector2 direction = (Vector2)(player.transform.position) - currPos;
        RaycastHit2D ray = Physics2D.Raycast(currPos, direction, aggroRange, 3);
        if (ray && (ray.collider.gameObject == player || (ray.collider.transform.parent != null && ray.collider.transform.parent.gameObject == player))) {
            return true;
        }
        return false;
    }

    protected override void Chase(){
        if (!canChase){ //if the enemy is unable to chase the player return
            return;
        }

        if (player.transform.position.x > transform.position.x) { 
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed * aggroMoveSpeedMultiplier, transform.GetComponent<Rigidbody2D>().velocity.y); //move to the right towards player
        }
        else {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-walkSpeed * aggroMoveSpeedMultiplier, transform.GetComponent<Rigidbody2D>().velocity.y); //move to the left towards player
        }
    }

    protected override bool IsWithinTetherRange(){
        if (transform.position.x >= currMaxXPaceDistance + tetherRange || transform.position.x <= currMinXPaceDistance - tetherRange) {
            anim.SetBool("Fight", false);
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
        if (transform.position.x >= currMaxXPaceDistance) {
            if (movingRight){
                SetCanMovePatrolling();
            }
            movingRight = false;
            //UpdatePatrolPoints();
        } 
        else if (transform.position.x <= currMinXPaceDistance) {
            if (!movingRight){
                SetCanMovePatrolling();
            }
            movingRight = true;
            UpdatePatrolPoints();
        }
    }

    private void CanChaseInvokeHelper(){
        canChase = true;
    }

    private void UpdatePatrolPoints(){
        currMinXPaceDistance = Random.Range(minXPaceDistance, Random.Range(0, maxXPaceDistance));
        currMaxXPaceDistance = Random.Range(currMinXPaceDistance, maxXPaceDistance);
    }

    private void SetCanMovePatrolling(){
        canMovePatrolling = false;
        CancelInvoke("PaceEndpointPauseDurationInvokeHelper");
        Invoke("PaceEndpointPauseDurationInvokeHelper", paceEndpointPauseDuration);

    }
    private void PaceEndpointPauseDurationInvokeHelper(){
        canMovePatrolling = true;
    }
}
