using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
     private Animator bombAnim;
    private Animator bombBarAnim;
    private Collider2D coll;
    private Rigidbody2D rb;

    public float startTime;
    public float waitTime;
    public float bombForce;

    [Header("check")]
    public float radius;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        bombAnim = GetComponent<Animator>();
        bombBarAnim = GetComponentInChildren<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (!bombAnim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                bombAnim.Play("explosion");
            }
        }
    }
     
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Explosion()
    {
        coll.enabled = false;

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        
        rb.gravityScale = 0;

        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((-pos + Vector3.up) * bombForce, ForceMode2D.Impulse);

            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            if(item.CompareTag("Player"))
            {
                item.GetComponent<IDamageable>().GetHit(3);   //这样写的好处就在于所有实现了这个接口的类    
                                                                                    //都可以进行一种同样的调用
            }
        }
    }

    public void DestoryThis()
    {
        Destroy(gameObject);
    }

    public void TurnOff()
    {
        startTime = Time.time;
        bombAnim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    public void TurnOn()
    {
        startTime = Time.time;
        bombAnim.Play("bomb_on");
        //bombBarAnim.Play("bomb_bar");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }

   
}

