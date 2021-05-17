using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;
    public float speed;
    public float jumpForce;
    [Header("Player State")]
    public float Health;
    public bool isDead;
    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("Status Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;
    [Header("FX")]
    public GameObject jumpFX;
    public GameObject landFX;
    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();
        GameManager.instance.playerRegister(this);
        Health = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(Health);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;

        checkInput();
        
    }
    public void FixedUpdate()
    {
        if (isDead) {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement();
        Jump();
    }
    void Movement() {
        float horizontalInput = joystick.Horizontal;

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        //if (horizontalInput!=0) {
        //    transform.localScale = new Vector3(horizontalInput, 1, 1);
        //}
        if (horizontalInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (horizontalInput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }
    void checkInput() {
        if (Input.GetButtonDown("Jump")&&isGround) {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            Attack();
        }
    }
    void Jump() {
        if (canJump) {
            isJump = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0); // The bias distance of the between jump FX apperance and player is -0.45
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 4;
            canJump = false;
        }
    }
    public void buttonJump()
    {
        if (isGround)
        {
            canJump = true;
        }
    }
    public void Attack() {
        if (Time.time > nextAttack) {
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation); //Generate a bomb
            nextAttack = Time.time + attackRate; // After generation, a cooldown will stop player from generate another bomb until the cooldown ends
        }
    }
    void PhysicsCheck() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround) {

            isJump = false;
        }
    
    }
    public void LandFX() { // Animation Event
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_hit"))
        {
            Health -= damage;
            if (Health < 1)
            {
                Health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");
            UIManager.instance.UpdateHealth(Health);
        }
    }
    //public void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    //}

    //OnDrawGizmos is used to visualize the judgement area of jumping
}
