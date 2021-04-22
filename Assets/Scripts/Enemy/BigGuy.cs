using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy,IDamageable
{
    public Transform pickupPonit;
    public float power;

    public void GetHit(float damage)
    {
       
            health = health - damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");
        }
    
    public void PickupBomb()
    {
        if(targetPoint.CompareTag("Bomb")&&!hasBomb)
        {
            targetPoint.gameObject.transform.position = pickupPonit.position;
            targetPoint.SetParent(pickupPonit);
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb = true;
        }
    }
   
    public void ThrowBomb()
    {
        if(hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent);
            if (FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x < 0)
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * power, ForceMode2D.Impulse) ;
            else
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * power, ForceMode2D.Impulse);
        }
        hasBomb = false;
    }
}
