using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankStates{ IDLE, WALKING, DAMAGED, DEAD}

public class TankPawn : DronePawn
{
    public GameObject target;
    public GameObject turret;
    public GameObject projectile;
    public GameObject spawnpoint;
    Rigidbody2D rb;
    TankStates states;
    float timer = 5.0f;
    float secondTimer = 4.0f;
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
        secondTimer -= Time.deltaTime;
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
        Vector3 targ = target.transform.position;
        targ.z = 0f;

        Vector3 objectPos = turret.transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        if (facingLeft)
        {
            turret.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            turret.transform.localScale = new Vector3(1, 1, 1);
        }
            turret.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (secondTimer < 0)
        {
            secondTimer = 4.0f;
            StartCoroutine(fire());
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(2, 10);
    }

    IEnumerator fire()
    {
        for (int i = 0; i < 3; i++)
        {
            Factory(projectile, spawnpoint.transform.position, spawnpoint.transform.rotation, controller);
            yield return new WaitForSeconds(0.25f);
        }

        yield return null;
    }
}
