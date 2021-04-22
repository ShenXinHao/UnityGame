using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy,IDamageable
{
    // Start is called before the first frame update
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

    public void SetOff()
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
