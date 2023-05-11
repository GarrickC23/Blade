using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private bool isFrozen;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen) {
            Vector3 playerPosition = player.transform.position;
            this.transform.position = new Vector3(playerPosition.x, playerPosition.y + 8, -10);
        }
    }

    public void FreezeFollow() {
        isFrozen = true;
    }

    public void UnfreezeFollow() {
        isFrozen = false;
    }
}
