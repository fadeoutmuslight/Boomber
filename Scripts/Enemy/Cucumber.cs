using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy, IDamageable
{
    
    public void GetHit(float damage)
    {
        Health -= damage;
        if (Health < 1) {
            Health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public void SetOff() { //animation event
        targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
