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
        if (!isDead)
        {
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
                StartCoroutine(isDamaged());
            }
            else if (states == TankStates.DEAD)
            {
                isDead = true;
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
        if (health <= 0)
        {
            isDead = true;
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

    IEnumerator deathAn()
    {
        tankAn.SetTrigger("Killed");
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    IEnumerator isDamaged()
    {
        tankAn.SetTrigger("IsDamaged");
        IgnoresDamage = true;
        yield return new WaitForSeconds(2f);
        IgnoresDamage = false;
    }
    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo = null, Controller Instigator = null)
    {

        health -= Value;
        if (health <= 0)
        {
            if (_controller)
            {
                _controller.RequestSpectate();
            }
            Debug.Log(gameObject.name + " was killed by " + Instigator.playerName + " ripripripripripripripriprip");
            isDead = true;
            StartCoroutine(deathAn());
        }
        else
        {
            StartCoroutine(isDamaged());
        }

        string DamageEventString = Source.ActorName + " " + EventInfo.DamageType.verb + " " + this.ActorName + " (" + Value.ToString() + " damage)";
        if (Instigator)
        {
            DamageEventString = Instigator.playerName + " via " + DamageEventString;
        }
        else
        {
            DamageEventString = "The World via " + DamageEventString;
        }
        DAMAGELOG(DamageEventString);


        return true;
    }
}
