using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        GameManager.instance.IsDoor(this);//观察者模式，gamemanager是以被动的方法得到这个物体
        collider.enabled = false;
    }

    // Update is called once per frame
    public void OpenDoor()
    {
        anim.Play("open");
        collider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            GameManager.instance.NextLevel();
        }
    }
}
