using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   public float maxHealth;
   public float Health;
   [HideInInspector]
   public bool isParrying = false;

   public Image[] hearts; 
   public Sprite fullHeart;
   public Sprite emptyHeart; 

   private void Start() {
      Health = maxHealth;
   }

   public void TakeDamage(float damage)
   {
      Health -= damage;
      for ( int i = 0; i < hearts.Length; i++ )
      {
         if ( i < Health )
         {
            hearts[i].sprite = fullHeart;
         }
         else
         {
            hearts[i].sprite = emptyHeart; 
         }
         if (Health <= 0){
            Destroy(gameObject);
         }
      }
      if (Health <= 0)
      {
         Destroy(gameObject);
      }
   }
}
