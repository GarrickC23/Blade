using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{

    public float posXPaceDistance;
    public float negXPaceDistance;

    [Header("Movement Speed")]
    public float walkSpeed;

    [Header("Aggro/Tether Range")]
    public float aggroRange; //range for the enemy to detect the player

    public float guardRange; //how far away the enemy will stop when approaching the player

    public float tetherRange; // max distance from enemy's starting position, left patrol pos, and right patrol pos

    [HideInInspector] public bool movingRight = true;

    [HideInInspector] public bool isPatrolling = true;
    
    protected GameObject player;

    protected abstract void Start();

    protected abstract void Update();

    protected abstract void EnemyWalk();

    protected abstract bool InAggroRange();

    protected abstract bool InGuardRange();

    protected abstract void Chase();

    protected abstract bool IsWithinTetherRange();

    protected abstract void Patrol();
}
