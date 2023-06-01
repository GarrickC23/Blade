using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{

    public float posXPaceDistance;
    public float negXPaceDistance;

    public float walkSpeed;

    public float aggroRange;

    public float tetherRange; // max distance from enemy's starting position, left patrol pos, and right patrol pos

    public bool movingRight = true;
    public bool isPatrolling = true;
    
    protected GameObject player;

    protected abstract void Start();

    protected abstract void Update();

    protected abstract void EnemyWalk();

    protected abstract bool InAggroRange();

    protected abstract void Chase();

    protected abstract bool IsWithinTetherRange();

    protected abstract void Patrol();
}
