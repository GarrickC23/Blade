using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static PlayerKnockback;

public class PlayerStats : MonoBehaviour
{
   public float maxHealth;
   public float health;
   public float InvulnerableDuration;
   public float InvulnerableFlashPeriod;
   [HideInInspector]
   public bool isParrying = false;

   [HideInInspector]
   public bool isGuarding = false; 

   [HideInInspector]
   public bool isStunned = false;

   [HideInInspector]
   public bool isKnockedBack = false;
   [HideInInspector]
   public bool isInvulnerable = false;

   

   [Header("Stagger Variables")]
   public float stagger = 0;
   public float maxStagger, staggerTime, staggerDecayRate, staggerDecayAmount, staggerDecayDelay;
   

   
   private Animator anim;

   
   public Transform respawnPoint;     // Where the player spawns after dying
   public Transform lastCheckpoint;   // Where the player spawns after falling off map

   private void Start() 
   {
      health = maxHealth;
      anim = gameObject.GetComponent<Animator>();
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
}
