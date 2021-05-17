using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        GameManager.instance.doorRegister(this);
        coll.enabled = false;
    }

    public void DoorOpen() //the function for calling by Game Manager
    {
        anim.Play("open");
        coll.enabled = true;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //if player's collider touch the one of door, the game manager will turn the game to the next stage
        {
            GameManager.instance.nextLevel();
        }
    }
}
