using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : Interactable
{
    private PlayerStats player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public override void Interact()
    {
        player.SetRespawnPoint(this.transform);
    }
}
