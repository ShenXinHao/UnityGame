using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStateBase currentState;
    public Animator anim;
    public int animState;
    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;


    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    private float nextAttack = 0;
    public float attackRange, skillRange;



    public List<Transform> attackList = new List<Transform>();
    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();




    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;

    }

    private void Awake()
    {
        Init();
    }


    void Start()
    {
        TransitionToState(patrolState);            //给currentState 进行赋初始值
        if (isBoss)
            UIManager.instance.SetBossHealth(health);
        GameManager.instance.IsEnemy(this);
    }


    public virtual void Update()
    {
        anim.SetBool("dead", isDead);
        if (isBoss)
            UIManager.instance.UpdateBossHealth(health);
        if (isDead)
        {
            GameManager.instance.EnemyDead(this);
            return;
        }

        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
        

    }

    public void TransitionToState(EnemyStateBase state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveTotarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();
    }


    public void AttackAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.transform.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
            }
        }

    }


    public virtual void SkillAction() //skill attack 
    {
        if (Vector2.Distance(transform.position, targetPoint.transform.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("skill");
                nextAttack = Time.time + attackRate;
            }
        }

    }

    public void FilpDirection()
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform) &&!hasBomb&& !GameManager.instance.gameOver && !isDead)
           attackList.Add(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && !GameManager.instance.gameOver)
            StartCoroutine(OnAlarm());
    }

    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}

