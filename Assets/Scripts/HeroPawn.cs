using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPawn : Pawn
{
    public float Health = 100.0f;
    public Rigidbody2D rb;
    public float Speed = 200f;
    public bool facingRight;
    public Vector3 theScale;


    public virtual void Horizontal(float value)
    {
        LOG("Horizontal:" + value);

        if (value == -1)
        {
            Debug.Log("MOVING LEFT");
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            rb.velocity = new Vector2(-1 * Speed * Time.deltaTime, rb.velocity.y);
            Flip();
        }
        if (value == 1)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            rb.velocity = new Vector2(Speed * Time.deltaTime, rb.velocity.y);
            Flip();
        }
        if (value == 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
        }
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        theScale = Vector3.zero;
        facingRight = true;
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
