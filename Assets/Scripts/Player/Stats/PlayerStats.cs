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

   public float stagger = 0, maxStagger, staggerTime;
   

   public Image[] hearts; 
   public Sprite fullHeart;
   public Sprite emptyHeart; 
   private Animator anim;

   private void Start() 
   {
      health = maxHealth;
      anim = gameObject.GetComponent<Animator>();
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
         anim.Play("BlockFlash");
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
      stagger += staggerIncrease;
      if (stagger >= maxStagger){
            stagger = 0;
            isStunned = true;
            Invoke("resetParry", staggerTime);
        }
   }

   private void resetParry(){
        isStunned = false;
    }
}
