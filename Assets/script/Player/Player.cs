using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumppower;

    int death = 0;

    float vx = 0;
    public static bool leftFlag = false;
    bool pushFlag = false;
    bool jumpFlag = false;
    bool groundFlag = false;

    bool Attacking = false;
    public static bool Attackmotion = false;

    Rigidbody2D rbody; // ������ٵ� �������� �ڵ�
    Animator anim; // �ִϸ����� �������� �ڵ�

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        death = 0;
    }

    void Update()
    {
        vx = 0;
        if (Input.GetKey("right"))
        {
            vx = speed;
            leftFlag = false;
            Invoke("AnimOn", 0);
        }
        if (Input.GetKey("left"))
        {
            vx = -speed;
            leftFlag = true;
            Invoke("AnimOn", 0);
        }
        if (Input.GetKeyUp("right"))
        {
            Invoke("AnimOff", 0);
        }
        if (Input.GetKeyUp("left"))
        {
            Invoke("AnimOff", 0);
        }
        if (Input.GetKey("space") && groundFlag)
        {
            if (pushFlag == false)
            {
                jumpFlag = true;
                pushFlag = true;
            }
        }
        else
        {
            pushFlag = false;
        }

        if(Input.GetKeyDown("a"))
        {
            if (!Attacking && groundFlag && !Attackmotion)
            {
                Attacking = true;
                anim.SetTrigger("isAttack");
                Invoke("Attack_ing", 0.73f);
                Invoke("Attack_motion1", 0.325f);
            }
        }
    }

    private void Attack_motion1()
    {
        Attackmotion = true;
        Invoke("Attack_motion2", 0.375f);
    }

    private void Attack_motion2()
    {
        Attackmotion = false;
    }

    private void Attack_ing()
    {
        Attacking = false;
    }

    private void AnimOn()
    {
        anim.SetBool("isRun", true);
    }

    private void AnimOff()
    {
        anim.SetBool("isRun", false);
    }

    void FixedUpdate()
    {
        if (!Attacking && (death == 0))
        {
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            rbody.velocity = new Vector2(vx, rbody.velocity.y);
            this.GetComponent<SpriteRenderer>().flipX = leftFlag;

            if (jumpFlag)
            {
                jumpFlag = false;
                rbody.AddForce(new Vector2(0, jumppower), ForceMode2D.Impulse);
            }
        }
        else if (Attacking || (death >= 0))
        {
            rbody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.tag == ("block"))
        {
            groundFlag = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        groundFlag = false;
    }

    public void Player_Dead()
    {
        if (death == 0)
        {
            death++;
            anim.SetTrigger("isDeath");
            this.GetComponent<BoxCollider2D>().enabled = false;
            Invoke("Player_Death", 1.35f);
        }
    }
    public void Player_Death()
    {
        Destroy(gameObject);
    }
}