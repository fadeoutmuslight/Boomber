using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bald : Enemy, IDamageable
{
    public void GetHit(float damage)
    {
        Health -= damage;
        if (Health < 1)
        {
            Health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }
    public void kickBomb()
    {
        Vector3 posBomb = targetPoint.GetComponent<Bomb>().transform.position;
        targetPoint.GetComponent<Bomb>().GetComponent<Rigidbody2D>().AddForce((-posBomb + Vector3.forward) * 2, ForceMode2D.Impulse);
    }
}
