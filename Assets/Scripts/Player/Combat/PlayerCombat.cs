using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject sparks;
    PlayerStats playerStatsRef;
    PlayerUI playerUIRef;
    private Animator anim;

    SpriteRenderer spriteRendererRef = null;
    private void Awake() {
        playerStatsRef = GetComponent<PlayerStats>();
        playerUIRef = GetComponent<PlayerUI>();
        anim = GetComponent<Animator>();
        
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)){
         spriteRendererRef = sr;
        }
    }
    
    //called by enemy when it wants to attack the player
    public void Attacked(float damage, float angle, float knockbackPower, float stunDuration, float playerParryIncrease, Transform attackerRefPos, float knockbackDuration)
   {
      if (!playerStatsRef.isParrying && !playerStatsRef.isStunned && !playerStatsRef.isGuarding && !playerStatsRef.isInvulnerable) //check to see if the player can be damaged
      {
         TakeDamage(damage);
         StartCoroutine(StunKnockbackInvulnerableCoroutine(angle, knockbackPower, attackerRefPos, stunDuration, knockbackDuration));
         if (attackerRefPos.gameObject.TryGetComponent<Projectile>(out Projectile tempProj)) {
            Destroy(attackerRefPos.gameObject);
         }
      }
      else if (playerStatsRef.isGuarding && !playerStatsRef.isParrying && !playerStatsRef.isStunned && !playerStatsRef.isInvulnerable) //check to see if the player is guarding
      {
         Debug.Log("Guard hit");
         IncreasePlayerStagger(playerParryIncrease);
         if (attackerRefPos.gameObject.TryGetComponent<Projectile>(out Projectile tempProj)) {
            Destroy(attackerRefPos.gameObject);
         }
      }
      else if (playerStatsRef.isParrying) //check to see if the player is parrying
      {
         // anim.Play("BlockFlash");
         Debug.Log("Parried");
         GameObject spark = Instantiate(sparks, transform.position, Quaternion.identity);
         spark.GetComponent<ParticleSystem>().Play();
         //Enemy stagger increases when player successfully parries the enemy's attack
         //attackerRefPos.gameObject.GetComponent<EnemyStats>().IncreaseStagger(attackerRefPos.gameObject.GetComponent<EnemyStats>().EnemyStaggerIncreaseOnPlayerParry);
         if (attackerRefPos.gameObject.TryGetComponent<EnemyStagger>(out EnemyStagger enemy)) {
            enemy.IncreaseStagger(enemy.GetComponent<EnemyStats>().EnemyStaggerIncreaseOnPlayerParry);
         }
         if (attackerRefPos.gameObject.TryGetComponent<Projectile>(out Projectile tempProj)) {
            if (tempProj.isReflectable) {
               tempProj.Bounce();
            }
         }
         //Destroy(spark, 2f);
      }
   }

   public void TakeDamage(float damage)
   {
      playerStatsRef.health -= damage;
      playerUIRef.HeartUpdate();

      if (playerStatsRef.health <= 0)
      {
         // Destroy(gameObject);
         playerStatsRef.health = playerStatsRef.maxHealth;
         playerUIRef.HeartUpdate();
         this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
         Vector3 oldPos = this.transform.position;
         this.transform.position = new Vector3(playerStatsRef.respawnPoint.position.x, playerStatsRef.respawnPoint.position.y, oldPos.z);
      }
   }

   public void IncreasePlayerStagger(float staggerIncrease){
      CancelInvoke("decayStagger");
      playerStatsRef.stagger += staggerIncrease;
      if (playerStatsRef.stagger >= playerStatsRef.maxStagger){
            playerStatsRef.isStunned = true;
            Invoke("resetStagger", playerStatsRef.staggerTime);
        }
        InvokeRepeating("decayStagger", playerStatsRef.staggerDecayDelay, playerStatsRef.staggerDecayRate);
   }

   private void resetStagger(){
        playerStatsRef.stagger = 0;
        playerStatsRef.isStunned = false;
   }

    private void decayStagger(){
      if (playerStatsRef.stagger > 0){
         playerStatsRef.stagger -= playerStatsRef.staggerDecayAmount;
      }
      if (playerStatsRef.stagger < 0){
         playerStatsRef.stagger = 0;
         CancelInvoke("decayStagger");
      }
   }
   
   private IEnumerator StunKnockbackInvulnerableCoroutine(float angle, float knockbackPower, Transform attackerRefPos, float stunDuration, float knockbackDuration){
        playerStatsRef.isStunned = true;
        anim.Play("HeroKnight_Hurt");
        yield return new WaitForSeconds(stunDuration);
        playerStatsRef.isStunned = false;
        SetInvulernable(); //gives the player i-frames after stun ends
        FlashInvulernable(); //flash the player to show invulnerability
        Invoke("SetVulnerable", playerStatsRef.InvulnerableDuration); //waits until i-frame duration before setting playing vulnerable again
        GetComponent<PlayerKnockback>().PlayerKnockbackFunction(knockbackPower, angle, attackerRefPos);
        playerStatsRef.isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDuration);
        playerStatsRef.isKnockedBack = false;
   }

   private void SetInvulernable(){
        playerStatsRef.isInvulnerable = true;
   }

   private void SetVulnerable(){
        playerStatsRef.isInvulnerable = false;
   }

   private void FlashInvulernable(){
        InvokeRepeating("FlashInverunableHelperInvoke",0, playerStatsRef.InvulnerableFlashPeriod);
        Invoke("FlashInvunerableHelperStop", playerStatsRef.InvulnerableDuration);
   }
   
   private void FlashInverunableHelperInvoke(){
        Color Color = spriteRendererRef.color;
        if (Color == Color.white){
            Color = Color.clear; //set player sprite visible
        }
        else{
            Color = Color.white; //set player sprite invisible
        }
        spriteRendererRef.color = Color;
   }

   private void FlashInvunerableHelperStop(){
        
        CancelInvoke("FlashInverunableHelperInvoke");
        spriteRendererRef.color = Color.white;
   }
}
