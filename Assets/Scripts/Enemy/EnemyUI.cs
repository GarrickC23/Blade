using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("Stagger UI")]
    public Slider staggerBarSlider;
    public Image staggerBarSliderFillColor;
    public Image staggerBarSliderBackgroundColor;

    EnemyStats enemyStatsRef;
    private void Awake() {
        enemyStatsRef = GetComponent<EnemyStats>();
    }

    private void Update() {
    staggerBarSlider.value = enemyStatsRef.stagger/enemyStatsRef.maxStagger;
      if (enemyStatsRef.stagger == 0){
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
}
