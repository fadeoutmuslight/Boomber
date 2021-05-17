using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D otherCollision)
    {
        if (otherCollision.CompareTag("Player")) {
            Debug.Log("!!!!");
            otherCollision.GetComponent<IDamageable>().GetHit(1);
        }
        if (otherCollision.CompareTag("Bomb")) { 
            
        }
    }
}
