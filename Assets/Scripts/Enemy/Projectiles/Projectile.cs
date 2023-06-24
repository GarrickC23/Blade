using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isReflectable;
    public float damage;
    public float knockbackPower;
    public float stunDuration;
    public float knockbackDuration;
    public float playerParryIncrease;
    public float speed;
    private Rigidbody2D rb;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        // rb.velocity = new Vector2(speed * Time.deltaTime, 0);
    }

    public void AddForce(Vector2 direction, float force) {
        direction = direction.normalized;
        rb.AddRelativeForce(direction * force, ForceMode2D.Impulse);
    }

    public void Bounce() {
        // rb.velocity = new Vector2(-speed * Time.deltaTime, 0);
        rb.velocity = -rb.velocity;
        Vector3 rot = this.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x,rot.y+180,rot.z);
        this.transform.rotation = Quaternion.Euler(rot);  
    }

    public void DestroyProjectile() {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player") {
            Debug.Log(coll.gameObject);
            if (coll.gameObject.TryGetComponent<PlayerCombat>(out PlayerCombat player)) {
                player.Attacked(damage, 0f, knockbackPower, stunDuration, playerParryIncrease, this.transform, knockbackDuration);
            }
        }
        else {
            Destroy(this.gameObject);
        }
    }
}
