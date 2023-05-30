using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyKnockback;

//Enemy should get stuff from EnemyStats
public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    public float health;

    [HideInInspector]
    public bool isParrying = true;
    
    [Header("Stagger Variables")]
    public float stagger = 0, maxStagger, parryResetTimer, staggerDecayAmount, staggerDecayRate, staggerDecayDelay;
    public float PlayerStaggerIncrease;
    public float EnemyStaggerIncreaseOnPlayerParry;

    [HideInInspector]
    public bool isKnockedBack = false;

    [HideInInspector]
    public bool isStunned = false;
    public bool isAttacking = false;

    

   [Header("Enemy Resistance Stats")]
    public float knockbackPowerMultiplier; //multiplies knockback received recieved by enemy knockback resistance
    public float stunDurationMultiplier; //multiplies stun duration recieved by enemy stun resistance
    public float damageTakenMultiplier; //multiples damagereceived by enemy damage resistance


    private void Start() 
    {
        health = maxHealth;
    }
}
