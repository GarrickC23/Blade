using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isReflectable;
    public float damage;
    public float knockbackPower;
    public float stunDuration;
    public float speed;
    private Rigidbody2D rb;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed * Time.deltaTime, 0);
    }

    public void Bounce() {
        rb.velocity = new Vector2(-speed * Time.deltaTime, 0);
        Vector3 rot = this.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x,rot.y+180,rot.z);
        this.transform.rotation = Quaternion.Euler(rot);  
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player") {
            if (coll.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player)) {
                player.Attacked(damage, 0f, knockbackPower, stunDuration, PlayerKnockback.Direction.AWAY, this.transform);
            }
        }
    }
}
