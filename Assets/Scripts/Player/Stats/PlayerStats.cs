using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;
using static PlayerKnockback;

public class PlayerStats : MonoBehaviour
{
   public float maxHealth;
   public float health;
   [HideInInspector]
   public bool isParrying = false;

   [HideInInspector]
   public bool isGuarding = false; 

   [HideInInspector]
   public bool isStunned = false;

   [HideInInspector]
   public bool isKnockedBack = false;

   [Header("Stagger UI")]
   public Slider staggerBarSlider;
   public Image staggerBarSliderFillColor;
   public Image staggerBarSliderBackgroundColor;

   [Header("Stagger Variables")]
   public float stagger = 0;
   public float maxStagger, staggerTime, staggerDecayRate, staggerDecayAmount, staggerDecayDelay;
   

   public Image[] hearts; 
   public Sprite fullHeart;
   public Sprite emptyHeart; 
   private Animator anim;

   public GameObject sparks;
   private Transform respawnPoint;     // Where the player spawns after dying
   private Transform lastCheckpoint;   // Where the player spawns after falling off map

   private void Start() 
   {
      health = maxHealth;
      anim = gameObject.GetComponent<Animator>();
   }

   private void LateUpdate() {
      //scuffed way of rotating the slider
      if(gameObject.transform.rotation.y == 180){
         staggerBarSlider.transform.parent.rotation = Quaternion.Euler(0,180,0);
      }
      else{
         staggerBarSlider.transform.parent.rotation = Quaternion.Euler(0,0,0);
      }
   }
   private void Update() {
      //update slider to match stagger value
      //if stagger is 0 hide the stagger bar
      staggerBarSlider.value = stagger/maxStagger;
      if (stagger == 0){
         staggerBarSliderFillColor.color = new Color(255,0,0,0);
         staggerBarSliderBackgroundColor.color = new Color(255,255,255,0);
      }
      else{
         staggerBarSliderFillColor.color = new Color(255,0,0,1);
         staggerBarSliderBackgroundColor.color = new Color(255,255,255,1);
      }

   }

   
   //called by enemy when it wants to attack the player
   public void Attacked(float damage, float angle, float knockbackPower, float stunDuration, Direction direction, Transform attackerRefPos)
   {
      if (!isParrying && !isStunned && !isGuarding)
      {
         TakeDamage(damage);
         GetComponent<PlayerKnockback>().PlayerKnockbackFunction(knockbackPower, angle, direction, this.transform, stunDuration);
         if (attackerRefPos.gameObject.TryGetComponent<Projectile>(out Projectile tempProj)) {
            Destroy(attackerRefPos.gameObject);
         }
      }
      else if (isGuarding && !isParrying && !isStunned)
      {
         Debug.Log("Guard hit");
         if (attackerRefPos.gameObject.TryGetComponent<Projectile>(out Projectile tempProj)) {
            Destroy(attackerRefPos.gameObject);
         }
      }
      else if (isParrying)
      {
         // anim.Play("BlockFlash");
         Debug.Log("Parried");
         GameObject spark = Instantiate(sparks, transform.position, Quaternion.identity);
         spark.GetComponent<ParticleSystem>().Play();
         if (attackerRefPos.gameObject.TryGetComponent<Projectile>(out Projectile tempProj)) {
            if (tempProj.isReflectable) {
               tempProj.Bounce();
            }
         }
         else {
            attackerRefPos.gameObject.GetComponent<EnemyStats>().IncreaseStagger(attackerRefPos.gameObject.GetComponent<EnemyStats>().EnemyStaggerIncreaseOnPlayerParry);
         }
         //Destroy(spark, 2f);
      }
   }

   
   public void TakeDamage(float damage)
   {
      health -= damage;
      HeartUpdate();

      if (health <= 0)
      {
         // Destroy(gameObject);
         health = maxHealth;
         HeartUpdate();
         this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
         Vector3 oldPos = this.transform.position;
         this.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, oldPos.z);
      }
   }

   public void HeartUpdate()
   {
      for ( int i = 0; i < hearts.Length; i++ )
      {
         if ( i < health )
         {
            hearts[i].sprite = fullHeart;
         }
         else
         {
            hearts[i].sprite = emptyHeart; 
         }
      }
   }

   public void IncreasePlayerStagger(float staggerIncrease){
      CancelInvoke("decayStagger");
      stagger += staggerIncrease;
      if (stagger >= maxStagger){
            isStunned = true;
            Invoke("resetStagger", staggerTime);
        }
        InvokeRepeating("decayStagger", staggerDecayDelay, staggerDecayRate);
   }

   public void SetRespawnPoint(Transform newRespawnPoint) {
      respawnPoint = newRespawnPoint;
   }

   public void SetCheckpoint(Transform newCheckpoint) {
      lastCheckpoint = newCheckpoint;
   }

   public void RespawnAtCheckPoint() {
      this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
      Vector3 oldPos = this.transform.position;
      this.transform.position = new Vector3(lastCheckpoint.position.x, lastCheckpoint.position.y, oldPos.z);
   }

   private void resetStagger(){
        stagger = 0;
        isStunned = false;
   }
   
   private void decayStagger(){
      if (stagger > 0){
         stagger -= staggerDecayAmount;
      }
      if (stagger < 0){
         stagger = 0;
         CancelInvoke("decayStagger");
      }
   }
}
