using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy, IDamageable
{
    SpriteRenderer sr;
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
    public override void Init()
    {
        base.Init();
        sr = GetComponent<SpriteRenderer>();

    }
    public override void Update()
    {
        base.Update();
        if (animState == 0) {
            sr.flipX = false;
        }
    }
    public override void SkillAction()
    {
        base.SkillAction();
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Captain_skill"))
        {
            sr.flipX = true;
            if (transform.position.x > targetPoint.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, speed * 2 * Time.deltaTime);
            }
        }
        else {
            sr.flipX = false;
        }
    }
}
