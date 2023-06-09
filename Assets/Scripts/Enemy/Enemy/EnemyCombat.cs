using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    float flashRedTime = 0.5f;
    EnemyStats enemyStats = null;
    EnemyStagger enemyStagger = null;

    SpriteRenderer spriteRenderer = null;
    public float knockbackDuration;
    private void Awake() {
        if (TryGetComponent<EnemyStats>(out EnemyStats enemystats)){
            enemyStats = enemystats;
        }
        if (TryGetComponent<EnemyStagger>(out EnemyStagger enemystagger)){
            enemyStagger = enemystagger;
        }
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)){
            spriteRenderer = sr;
        }
        
    }
    public void EnemyAttacked(float damage, float angle, float knockbackPower, float stunDuration, Transform attackerPos)
    {
        if (enemyStats.isParrying){
            enemyStagger.IncreaseStagger(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().IncreasePlayerStagger(enemyStats.PlayerStaggerIncrease);
            StartCoroutine(KnockbackCoroutine());
            GetComponent<EnemyKnockback>().EnemyKnockbackFunction(knockbackPower * enemyStats.knockbackPowerMultiplier, angle, attackerPos);
        }
        else 
        {
            StartCoroutine(StunCoroutine(knockbackPower, angle, attackerPos, stunDuration));

            TakeDamage(damage * enemyStats.damageTakenMultiplier);
        }
    }

    private void TakeDamage(float damage)
    {
        enemyStats.health -= damage;
        FlashRed();
        if (enemyStats.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StunCoroutine(float knockbackPower, float angle, Transform XerencePoint, float stunDuration)
    {
        
        enemyStats.isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        enemyStats.isStunned = false;
    }

    private IEnumerator KnockbackCoroutine(){
        enemyStats.isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDuration);
        enemyStats.isKnockedBack = false;
    }


    private void FlashRed(){
        spriteRenderer.color = Color.red;
        Invoke("FlashRedHelperStop", flashRedTime);
    }

   private void FlashRedHelperStop(){
        spriteRenderer.color = Color.white;
   }
}
