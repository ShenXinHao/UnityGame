using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
   private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick; 
    public float speed;
    public bool isHurt;
    public float jumpForce;

    [Header("Player State")] 
    public float health;
    public bool isDead;


    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;


    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;


    [Header("Jump FX")]
    public GameObject landFX;
    public GameObject jumpFX;


    [Header("Attack Settings")]
    public GameObject BombPrefabs;
    public float nextAttack = 0;
    public float attackRate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();
        GameManager.instance.IsPlayer(this);
        health = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
         anim.SetBool("dead", isDead);
        if (isDead)
            return;
        //用来判断是否正在播放受伤动画
        isHurt = anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit");
        CheckInput();
    }



    public void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        

        PhysicsCheck();
        //非受伤状态才可以移动和跳跃

        if (!isHurt)
        {
            Movement();//input会覆盖Rigidbody的速度，所以用isHurt来锁定就可以让 Player 被击飞了
            Jump();
        }
        
       
    }

            void Movement()
            {
                //键盘操作
                //float horizontalInput = Input.GetAxis("Horizontal");        //-1~+1包括小数
                //float horizontalInput = Input.GetAxisRaw("Horizontal");     //不包括小数

                //操纵杆
                float horizontalInput = joystick.Horizontal;
                rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // if(horizontalInput!=0)
        // transform.localScale = new Vector3(horizontalInput, 1, 1);
        if (horizontalInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (horizontalInput < 0)
            transform.eulerAngles = new Vector3(0, -180, 0);

            }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump")&&isGround)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }


   void Jump()
    {
        if(canJump)
        {
            isJump = true;
           jumpFX.SetActive(true);
           jumpFX.transform.position = transform.position + new Vector3(0, -0.5f,0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 4;
            canJump = false;
          
        }
    }

    public void ButtonJump()
    {
        if(isGround)
        canJump = true;
    }

    public void Attack()
    {
        if(Time.time>nextAttack)
        {
            Instantiate(BombPrefabs, transform.position, BombPrefabs.transform.rotation);
            nextAttack = Time.time + attackRate;
        }
    }
    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer); //物体以周围圆形检测是否重叠
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;
        }
    }

    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.745f, 0);
    }


     public void OnDrawGizmos()
     {
         Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
     }


    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))//StateInfo括号中的序号对应着unity中动画层
        {

            health = health - damage;

            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");

             UIManager.instance.UpdateHealth(health);
        }
    }
}
