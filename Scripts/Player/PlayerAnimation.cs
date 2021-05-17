using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed",Mathf.Abs(rb.velocity.x)); //Here is minus speed value, Mathf.Abs used to make the value become a positive value to fulfill the requirement of animation changing
        anim.SetFloat("velocityY", rb.velocity.y); //No need to use Abs as the Y-axis speed can be negative to simulate gravity
        anim.SetBool("jump", controller.isJump);
        anim.SetBool("ground", controller.isGround);
    }
}
