using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    public GameObject drop;
    public int dropLowerBound;
    public int dropUpperBound;
    public void dropItem(){
        int dropCount = Random.Range(dropLowerBound, dropUpperBound + 1); //1 - 3
        for (int i = 0; i < dropCount; i++){
            GameObject newCoin = Instantiate(drop, transform.position, transform.rotation);
        }
    }
}
