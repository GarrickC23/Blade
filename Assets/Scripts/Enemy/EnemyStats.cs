using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyKnockback;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    float health;
    public float stagger = 0, maxStagger, parryResetTimer, staggerDecayAmount, staggerDecayRate, staggerDecayDelay;
    bool isParrying = true;
    public float PlayerStaggerIncrease;

    [HideInInspector]
    public bool isKnockedBack = false;

    [HideInInspector]
    public bool isStunned = false;

    [Header("Stagger UI")]
   public Slider staggerBarSlider;
   public Image staggerBarSliderFillColor;
   public Image staggerBarSliderBackgroundColor;

    private void Start() 
    {
        health = maxHealth;
    }

    private void Update() {
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

    private void LateUpdate() {
      //scuffed way of rotating the slider
      if(gameObject.transform.rotation.y == 180){
         staggerBarSlider.transform.parent.rotation = Quaternion.Euler(0,180,0);
      }
      else{
         staggerBarSlider.transform.parent.rotation = Quaternion.Euler(0,0,0);
      }
   }

    public void EnemyAttacked(float damage, float angle, float knockbackPower, float stunDuration, Direction direction)
    {
        if (isParrying){
            IncreaseStagger(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().IncreasePlayerStagger(PlayerStaggerIncrease);
            GetComponent<EnemyKnockback>().EnemyKnockbackFunction(knockbackPower, angle, direction, this.transform, stunDuration);
        }
        else 
        {
            GetComponent<EnemyKnockback>().EnemyKnockbackFunction(knockbackPower, angle, direction, this.transform, stunDuration);
            TakeDamage(damage);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void IncreaseStagger(float staggerIncrease){
        CancelInvoke("decayStagger");
        stagger += staggerIncrease;
        if (stagger >= maxStagger){
            stagger = 0;
            isParrying = false;
            Invoke("resetParry", parryResetTimer);
        }
        InvokeRepeating("decayStagger", staggerDecayDelay, staggerDecayRate);
    }

    private void resetParry(){
        isParrying = true;
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
