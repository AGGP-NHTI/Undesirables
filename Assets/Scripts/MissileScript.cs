using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : Actor
{
    public List<Collider2D> triggerList;
    public Collider2D playerFinder;

    GameObject player;
    float moveSpeed = 5f;
    float closeRange = .85f;
    Vector2 moveDirection;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

   void Update()
    {
        if(player)
        {
            updateMoveDir();

            move();

            Vector3 vectorToTarget = player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * moveSpeed);
        }
        
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        HeroPawn OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (OtherActor)
        {
            LOG("Help");
            player = other.gameObject;

            playerFinder.enabled = !playerFinder.enabled;
        }
    }

    float getDist()
    {
        float distance = Vector2.Distance(player.transform.position, gameObject.transform.position);

        return distance;
    }

    bool isCloseToPlayer()
    {
        float dist = getDist();

        return dist < closeRange;
    }

    void updateMoveDir()
    {
        Vector2 target = player.transform.position;
        Vector2 thisOb = gameObject.transform.position;

        moveDirection = (target - thisOb).normalized;
    }

    void move()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

}
