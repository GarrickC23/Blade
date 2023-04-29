using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    float health;
    public float stagger = 0, maxStagger, parryResetTimer;
    bool isParrying = true;
    public float PlayerStaggerIncrease;

    public bool isKnockedBack = false;

    private void Start() 
    {
        health = maxHealth;
    }

    public void EnemyAttacked(float damage, float angle, float knockbackPower, float stunDuration, Direction direction)
    {
        if (isParrying){
            IncreaseStagger(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().IncreasePlayerStagger(PlayerStaggerIncrease);
        }
        else 
        {
            GetComponent<EnemyKnockback>().EnemyKnockbackFunction(knockbackPower, angle, direction, this.transform, stunDuration);
            TakeDamage(damage);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void IncreaseStagger(float staggerIncrease){
        stagger += staggerIncrease;
        if (stagger >= maxStagger){
            stagger = 0;
            isParrying = false;
            Invoke("resetParry", parryResetTimer);
        }
    }

    private void resetParry(){
        isParrying = true;
    }


}
