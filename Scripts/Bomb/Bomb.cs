using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D myColl;
    private Rigidbody2D myRB;

    public float startTime;
    public float waitTime;
    public float bombForce;
    [Header("Check List")]
    public float radius;
    public LayerMask targetLayer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myColl = GetComponent<Collider2D>();
        myRB = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("bomb_explosion");

            }
        }
        Roll();
    }
    //public void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}
    public void Explosion() {// animation event
        Collider2D[] aroundObject = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer); //Used to decide what objects will be affected by bomb explosion
        myColl.enabled = false;
        myRB.gravityScale = 0;
        foreach (var item in aroundObject) {
            Vector3 pos = transform.position - item.transform.position; //Get the relative position of the object to the bomb
            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up) * bombForce, ForceMode2D.Impulse); //Giving a reversed impulsive force to the object affected.Vector3.up is simulate the up direction force
            if (item.CompareTag("Bomb")) {
                if (item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off")) {
                    item.GetComponent<Bomb>().TurnOn();
                }
            }
            if (item.CompareTag("Player")) {
                item.GetComponent<IDamageable>().GetHit(1);
            }
            if (item.CompareTag("Enemy"))
            {
                item.GetComponent<IDamageable>().GetHit(3);
            }
        }
    }
    public void Roll()
    {
        if (GameManager.instance.bombRollMode)
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.acceleration.x;
            this.GetComponent<Rigidbody2D>().AddForce(dir*3, ForceMode2D.Force);
        }
    }
    public void DestroyBomb() {
        Destroy(gameObject);
    }
    public void TurnOff() {
        anim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("Characters");
    }
    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }
}
