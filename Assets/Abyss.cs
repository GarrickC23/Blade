using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    public int damageDealt;
    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            PlayerStats player = coll.gameObject.GetComponent<PlayerStats>();
            if (player.health > damageDealt) {
                player.RespawnAtCheckPoint();
            }
            player.TakeDamage(1);
        }
    }
}
