using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerUI : MonoBehaviour
{
    public Image[] hearts; 
    public Sprite fullHeart;
    public Sprite emptyHeart; 

    [Header("Stagger UI")]
    public Slider staggerBarSlider;
    public Image staggerBarSliderFillColor;
    public Image staggerBarSliderBackgroundColor;

    PlayerStats playerStatsRef;
    private void Awake() {
        playerStatsRef = GetComponent<PlayerStats>();
    }

    private void Update() {
      //update slider to match stagger value
      //if stagger is 0 hide the stagger bar
      staggerBarSlider.value = playerStatsRef.stagger/playerStatsRef.maxStagger;
      if (playerStatsRef.stagger == 0){
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

   public void HeartUpdate()
   {
      for ( int i = 0; i < hearts.Length; i++ )
      {
         if ( i < playerStatsRef.health )
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
