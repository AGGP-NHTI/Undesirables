using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPawn : Pawn
{
    public float Health = 100.0f;
    public GameObject grenadePrefab;
    public GameObject grenadeSpawnLoc;
    public Rigidbody2D rb;
    public float Speed = 50f;
    private float attackCoolDwn = 0.35f;
    private float ProjattackCoolDwn = 0.75f;
    public bool facingRight;
    private bool ismAing = false;
    private bool ispAing = false;
    private bool ispAingInAir = false;
    private bool ismAingInAir = false;
    public bool inAir = false;
    public Vector3 theScale;
    public float jumpForce = 500f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        theScale = Vector3.zero;
        facingRight = true;
    }

    private void Update()
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

        if (ispAing)
        {
            if (ProjattackCoolDwn > 0f)
            {
                ProjattackCoolDwn -= Time.deltaTime;
            }
            else
            {
                if (ispAingInAir)
                {
                    ProjattackCoolDwn = 0.50f;
                    gameObject.GetComponent<Animator>().SetBool("ProjInAir", false);
                    ispAing = false;
                    ispAingInAir = false;
                }
                else
                {
                    ProjattackCoolDwn = 0.50f;
                    gameObject.GetComponent<Animator>().SetBool("isProjAtt", false);
                    ispAing = false;
                }
            }
        }

    }

    public virtual void Horizontal(float value)
    {

        if (value == -1)
        {
            Debug.Log("MOVING LEFT");
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            rb.velocity = new Vector2(-1 * Speed, rb.velocity.y);
            Flip();
        }
        if (value == 1)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            rb.velocity = new Vector2(1 * Speed, rb.velocity.y);
            Flip();
        }
        if (value == 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public virtual void Fire1(bool value)
    {
        if ((value) && (inAir == false))
        {
            Jump();
        }
    }

    public virtual void Fire2(bool value)
    {
        if ((value) && (ismAing == false))
        {
            mAttack();
        }
    }

    public virtual void Fire3(bool value)
    {
        if ((value) && (ispAing == false))
        {
            pAttack();
        }
    }

    void Jump()
    {
        gameObject.GetComponent<Animator>().SetBool("justJumped", true);
        rb.velocity = rb.velocity + (Vector2.up * jumpForce);
        inAir = true;
    }

    void Flip()
    {
        if (((rb.velocity.x < 0f) && (facingRight == true)) || ((rb.velocity.x > 0f) && (facingRight == false)))
        {
            facingRight = !facingRight;
            theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void mAttack()
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

    void pAttack()
    {
        if (inAir)
        {
            ispAingInAir = true;
            ispAing = true;
            gameObject.GetComponent<Animator>().SetBool("ProjInAir", true);
        }
        else
        {
            ispAing = true;
            gameObject.GetComponent<Animator>().SetBool("isProjAtt", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            gameObject.GetComponent<Animator>().SetBool("justJumped", false);
            inAir = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        LOG("Took Damage");
        Controller controller = GetController();
        Health = Health - Value;
        if (Health <= 0f)
        {
            Debug.Log(gameObject.name + " has died!");
            IgnoresDamage = true;
            //Game.Self.PlayerDied(this, Source, EventInfo, Instigator);
        }

        return false;
    }
}
