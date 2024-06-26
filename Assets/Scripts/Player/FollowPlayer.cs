using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private bool isFrozen;
    private bool isShaking;
    private float currDuration;
    private float currIntensity;
    private float shakeTimer;
    private float directionTimer;
    private float direction;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        // CallShakeScreen(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen) {
            Vector3 playerPosition = player.transform.position;
            this.transform.position = new Vector3(playerPosition.x, playerPosition.y + 4, -10);
        }
        if (isShaking) {
            if (shakeTimer < currDuration) {
                this.gameObject.transform.position += new Vector3(direction * currIntensity, 0, 0);
                if (directionTimer >= 0.05f) {   // 0.05f is the time between switching directions for shaking
                    direction = -direction;
                    directionTimer = 0;
                }
                else {
                    directionTimer += Time.deltaTime;
                }
                shakeTimer += Time.deltaTime;
            }
            else {
                isShaking = false;
                shakeTimer = 0;
            }
        }
    }

    public void FreezeFollow() {
        isFrozen = true;
    }

    public void UnfreezeFollow() {
        isFrozen = false;
    }

    /// <summary>
    /// Shakes the screen horizontally based around the player
    /// </summary>
    /// <param name="shakeTime">How long the shaking lasts</param>
    /// <param name="intensity">How far horizontally the screen goes</param>
    public void CallShakeScreen(float shakeTime, float intensity) {
        currDuration = shakeTime;
        currIntensity = intensity;
        isShaking = true;
        direction = 1;
    }
}
