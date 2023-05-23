using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStagger : MonoBehaviour
{
    private EnemyStats enemyStatsRef;

    private void Awake() {
        enemyStatsRef = GetComponent<EnemyStats>();
    }
    public void IncreaseStagger(float staggerIncrease){
        CancelInvoke("decayStagger");
        enemyStatsRef.stagger += staggerIncrease;
        if (enemyStatsRef.stagger >= enemyStatsRef.maxStagger){
            enemyStatsRef.isParrying = false;
            enemyStatsRef.isStunned = true;
            Invoke("resetParry", enemyStatsRef.parryResetTimer);
        }
        InvokeRepeating("decayStagger", enemyStatsRef.staggerDecayDelay, enemyStatsRef.staggerDecayRate);
    }

    private void resetParry(){
        enemyStatsRef.isParrying = true;
        enemyStatsRef.stagger = 0;
        enemyStatsRef.isStunned = false;
    }

    private void decayStagger(){
      if (enemyStatsRef.stagger > 0){
        enemyStatsRef.stagger -= enemyStatsRef.staggerDecayAmount;
      }
      if (enemyStatsRef.stagger < 0){
        enemyStatsRef.stagger = 0;
        CancelInvoke("decayStagger");
      }
   }
}
