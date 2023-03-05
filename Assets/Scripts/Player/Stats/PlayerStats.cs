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
   public bool isStunned = false;

   public Image[] hearts; 
   public Sprite fullHeart;
   public Sprite emptyHeart; 
   private Animator anim;

   private void Start() 
   {
      health = maxHealth;
      anim = gameObject.GetComponent<Animator>();
   }
   public void Attacked(float damage){
      if (!isParrying && !isStunned)
         {
            TakeDamage(damage);
            GetComponent<PlayerKnockback>().PlayerKnockbackFunction(30, 80, Direction.AWAY, this.transform, 0.4f);
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
}
