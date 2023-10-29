using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){
            GameObject player = other.gameObject;
            player.GetComponent<PlayerStats>().coinCount++;
            Destroy(gameObject);
        }
    }
}
