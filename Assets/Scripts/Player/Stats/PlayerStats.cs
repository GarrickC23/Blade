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

   public void Attacked(float damage, float angle, float knockbackPower, float stunDuration, Direction direction)
   {
      if (!isParrying && !isStunned && !isGuarding)
      {
         TakeDamage(damage);
         GetComponent<PlayerKnockback>().PlayerKnockbackFunction(knockbackPower, angle, direction, this.transform, stunDuration);
      }
      else if (isGuarding && !isParrying && !isStunned)
      {
         Debug.Log("Guard hit");
      }
      else if (isParrying)
      {
         // anim.Play("BlockFlash");
         GameObject spark = Instantiate(sparks, transform.position, Quaternion.identity);
         spark.GetComponent<ParticleSystem>().Play();
         //Destroy(spark, 2f);
         Debug.Log("Parried");
      }
   }

   private void TakeDamage(float damage)
   {
      health -= damage;
      HeartUpdate();

      if (health <= 0)
      {
         Destroy(gameObject);
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
      }
   }
}
