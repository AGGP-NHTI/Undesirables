using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{

    Rigidbody2D rb;
    public float Speed;
    public bool left = false;
    public bool right = false;
    public bool jump = false;
    private bool canJump = true;
    public bool mattack = false;
    public bool pattack = false;
    public bool facingRight;
    private bool ismAing = false;
    private bool ismAingInAir = false;
    private bool ispAing = false;
    private bool ispAingInAir = false;
    private bool inAir = false;
    public Vector3 theScale;
    public float jumpForce = 5f;
    private float attackCoolDwn = 0.35f;

    void Start()
    {
        Speed = 200f;
        //Physics.gravity = new Vector3(0f, -18f, 0f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        theScale = Vector3.zero;
        facingRight = true;        
    }

    void FixedUpdate()
    {
        HeroInput();
    }

    void HeroInput()
    {
        if (ismAing)
        {
            if (attackCoolDwn > 0f)
            {
                attackCoolDwn -= Time.deltaTime;
            }
            else
            {
                if (ismAingInAir)
                {
                    attackCoolDwn = 0.35f;
                    gameObject.GetComponent<Animator>().SetBool("attackInAir", false);
                    ismAing = false;
                    ismAingInAir = false;
                }
                else
                {
                    attackCoolDwn = 0.35f;
                    gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
                    ismAing = false;
                }
            }
        }
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
        jump = Input.GetKey(KeyCode.Space);
        mattack = Input.GetKey(KeyCode.W);
        pattack = Input.GetKey(KeyCode.E);



        if (left)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            rb.velocity = new Vector2(-1 * Speed * Time.deltaTime, rb.velocity.y);
            theScale.x *= -1;
            Flip();
        }
        if (right)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            rb.velocity = new Vector2(1 * Speed * Time.deltaTime, rb.velocity.y);
            theScale.x *= -1;
            Flip();
        }
        if ((!left) && (!right))
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if ((mattack) && (ismAing == false))
        {

            mAttack();
        }

        if ((pattack) && (ispAing == false))
        {
            pAttack();
        }

        if ((jump) && (canJump))
        {
            Jump();
        }
    }

    void Flip()
    {
        if (((rb.velocity.x < 0f) && (facingRight == true)) || ((rb.velocity.x > 0f) && (facingRight == false)))
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    void Jump()
    {
        gameObject.GetComponent<Animator>().SetBool("justJumped", true);
        rb.velocity = rb.velocity + (Vector2.up * jumpForce);
        Debug.Log("HJGJJJ");
        inAir = true;
        canJump = false;

    }

    void mAttack()
    {
        if (ismAing == false)
        {
            if (inAir)
            {
                gameObject.GetComponent<Animator>().SetBool("attackInAir", true);
                ismAing = true;
                ismAingInAir = true;
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
                ismAing = true;
            }
        }
    }

    void pAttack()
    {
        if (inAir)
        {

        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isProjAtt", true);
            Debug.Log("IT WORKED");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            gameObject.GetComponent<Animator>().SetBool("justJumped", false);
            canJump = true;
            inAir = false;
        }
    }


}
