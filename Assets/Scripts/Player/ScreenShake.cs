using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public GameObject cam;
    public bool isShaking = false;
    public float timer = 5;
    private FollowPlayer followScript;

    void Start() {
        timer = 5;
        followScript = cam.GetComponent<FollowPlayer>();
    }

    private void Update() {
        if (timer >= 5) {
            CallShakeScreen(1, 100);
            timer = 0;
        }
        else {
            timer += Time.deltaTime;
        }
    }

    public void CallShakeScreen(float shakeTime, float intensity) {
        if (!isShaking) {
            StartCoroutine(ShakeScreen(shakeTime, intensity));
        }
    }

    public IEnumerator ShakeScreen(float shakeTime, float intensity){
        isShaking = true;
        followScript.FreezeFollow();

        Vector3 origPos = cam.transform.position;
        Rigidbody2D cambody = cam.GetComponent<Rigidbody2D>();
        float shakeTimer = 0;
        float timeBetweenShakes = 0.1f;
        float directionTimer = 0;

        cambody.velocity = new Vector2(intensity * Time.deltaTime, 0);
        while (shakeTimer < shakeTime){
            if (directionTimer >= timeBetweenShakes){
                cambody.velocity = -cambody.velocity;
                directionTimer = 0;
            }
            directionTimer +=  Time.deltaTime;
            shakeTimer += Time.deltaTime;
            yield return null;
        }

        cambody.velocity = Vector2.zero;
        cam.transform.position = origPos;
        isShaking = false;
        followScript.UnfreezeFollow();
    }
}
