using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;
    public Animator anim;
    public int animState;//For control animator's state machine

    private GameObject alarmSign;
    private Rigidbody2D rb;
    private Collider2D myColl;
    [Header("Basic State")]
    public float Health;
    public bool isDead;
    public bool isBoss;
    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;
    [Header("Attack Settings")]
    public float attackRate;
    public float attackRange, skillRange;

    private float nextAttack = 0.5f;
    public List<Transform> attackList = new List<Transform>();

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init() {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject; //For getting the first child object of the enemy, which is the alarm sign.
    }
    public void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        myColl = GetComponent<Collider2D>();
        GameManager.instance.enemyRegister(this);
        transitionToState(patrolState);
        if (isBoss) {
            UIManager.instance.bossHealthBarWithBackground.SetActive(true);
            UIManager.instance.SetBossHealth(Health);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isBoss)
        {
            UIManager.instance.UpdateBossHealth(Health);
        }
        anim.SetBool("dead", isDead);
        if (isDead) {
            GameManager.instance.enemyDead(this);
            return;
        }
        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);//To control entering various animator state
    }
    public void transitionToState(EnemyBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }
    public void MoveToTarget() {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();    
    }
    public void AttackAction() {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack )
            {
                anim.SetTrigger("attack");
                //Debug.Log("1145141919810");
                nextAttack =Time.time+ attackRate;
            }
        }
    }
    public virtual void SkillAction() {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("skill");
                //Debug.Log("364364");
                nextAttack = Time.time + attackRate;
            }
        }
    }
    public void FilpDirection() {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    public void changeTarget() {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else {
            targetPoint = pointB;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform)&& !isDead && !GameManager.instance.gameOver) {
            attackList.Add(collision.transform);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        
        attackList.Remove(collision.transform);  
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead&&!GameManager.instance.gameOver)
        {
            StartCoroutine(OnAlarm());
        }
    }
    IEnumerator OnAlarm() {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);//Multithread running of alarm sign animation. 
        //(0) is the layer,[0] is the clip position
        alarmSign.SetActive(false);
    }
}
