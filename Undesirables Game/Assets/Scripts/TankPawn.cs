using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankStates{ IDLE, WALKING, DAMAGED, DEAD}

public class TankPawn : DronePawn
{

    Rigidbody2D rb;
    TankStates states;
    float timer = 5.0f;
    bool isDead;
    public Animator tankAn;
    bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        states = TankStates.IDLE;
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (states == TankStates.WALKING)
        {
            if (facingLeft)
            {
                rb.velocity = new Vector2(0.5f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-0.5f, rb.velocity.y);
            }
        }
        else if (states == TankStates.DAMAGED)
        {

        }
        else if (states == TankStates.DEAD)
        {

        }
        else
        {

        }
        if (timer < 0 && isDead != true)
        {
            ResetTimer();
            if (states == TankStates.WALKING)
            {
                tankAn.SetInteger("ChangeState", 0);
                states = TankStates.IDLE;
            }
            else
            {
                tankAn.SetInteger("ChangeState", 1);
                states = TankStates.WALKING;
                Vector2 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                facingLeft = !facingLeft;
            }
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(2, 10);
    }
}
