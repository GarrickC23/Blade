using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    float flashRedTime = 0.5f;
    EnemyStats enemyStatsRef = null;
    EnemyStagger enemyStaggerRef = null;

    SpriteRenderer spriteRendererRef = null;
    float knockbackDuration = 0.25f; //fixed knockback duration for enemies
    private void Awake() {
        if (TryGetComponent<EnemyStats>(out EnemyStats enemystats)){
            enemyStatsRef = enemystats;
        }
        if (TryGetComponent<EnemyStagger>(out EnemyStagger enemystagger)){
            enemyStaggerRef = enemystagger;
        }
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)){
            spriteRendererRef = sr;
        }
        
    }
    public void EnemyAttacked(float damage, float angle, float knockbackPower, float stunDuration, Transform attackerRefPos)
    {
        if (enemyStatsRef.isParrying){
            enemyStaggerRef.IncreaseStagger(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().IncreasePlayerStagger(enemyStatsRef.PlayerStaggerIncrease);
            StartCoroutine(KnockbackCoroutine());
            GetComponent<EnemyKnockback>().EnemyKnockbackFunction(knockbackPower * enemyStatsRef.knockbackPowerMultiplier, angle, attackerRefPos);
        }
        else 
        {
            StartCoroutine(StunCoroutine(knockbackPower, angle, attackerRefPos, stunDuration));

            TakeDamage(damage * enemyStatsRef.damageTakenMultiplier);
        }
    }

    private void TakeDamage(float damage)
    {
        enemyStatsRef.health -= damage;
        FlashRed();
        if (enemyStatsRef.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StunCoroutine(float knockbackPower, float angle, Transform XReferencePoint, float stunDuration)
    {
        
        enemyStatsRef.isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        enemyStatsRef.isStunned = false;
    }

    private IEnumerator KnockbackCoroutine(){
        enemyStatsRef.isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDuration);
        enemyStatsRef.isKnockedBack = false;
    }


    private void FlashRed(){
        spriteRendererRef.color = Color.red;
        Invoke("FlashRedHelperStop", flashRedTime);
    }

   private void FlashRedHelperStop(){
        spriteRendererRef.color = Color.white;
   }
}
