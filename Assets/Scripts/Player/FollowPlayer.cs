using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player; 

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        this.transform.position = new Vector3(playerPosition.x, playerPosition.y + 10, -10);
    }
}
