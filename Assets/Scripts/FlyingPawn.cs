using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyingStates { IDLE, MOVING, GUN, BLADE, DAMAGED, DEAD }

public class FlyingPawn : DronePawn
{

    Rigidbody2D rb;
    FlyingStates states;
    public float flyHeight;
    float timer = 5.0f;
    bool isDead;
    public Animator flyAn;
    public GameObject ground;
    bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        states = FlyingStates.IDLE;
        isDead = false;
        facingLeft = true;
        flyHeight = Random.Range(.75f, 3.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        
        if (states == FlyingStates.MOVING)
        {
                rb.velocity = new Vector2(0.5f, rb.velocity.y);
        }
        if (transform.position.y - ground.transform.position.y < flyHeight)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.5f);
        }

        if (timer < 0 && isDead != true)
        {
            ResetTimer();
            if (states == FlyingStates.MOVING)
            {
                flyAn.SetInteger("MoveState", 0);
                Debug.Log("Changing States");
                states = FlyingStates.IDLE;
            }
            else
            {
                flyAn.SetInteger("MoveState", 1);
                Debug.Log("Changing States");
                states = FlyingStates.MOVING;
                Vector2 theScale = transform.localScale;
                theScale.x *= -1;
                facingLeft = !facingLeft;
                transform.localScale = theScale;
            }
        }
    }
    void ResetTimer()
    {
        timer = 5.0f;
    }
}
