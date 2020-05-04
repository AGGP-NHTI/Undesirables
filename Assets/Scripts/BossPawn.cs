using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPawn : Pawn
{
    delegate void States();

    States currentState;

    Rigidbody2D rb;

    public Animator animator;
    public GameObject player;
    public GameObject spawnpoint;
    public GameObject flyingDrone;
    public GameObject groundDrone;
    Collider2D footHitbox;
    Collider2D leftSwordHitbox;
    Collider2D rightSwordHitbox;

    public bool bossHasDied = false;

    Vector2 moveDirection;
    public float moveSpeed = 5f;
    float flyingDroneCount = 0f;
    float groundDroneCount = 0f;
    float closeRange = 2f;
    float attackTime = 2f;
    float hitStunTime = 3f;
    float criticalHitStunTime = 5f;
    float missileTimer = 10f;//10 test num, raise to 20*
    float missileTimerStart = 0f;
    float timerStart = 0f;
    protected float startingHealth = 5000f;
    protected float currentHealth = 5000f;
    bool facingRight = false;
    bool isAttacking = false;
    Vector3 theScale;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        slider.minValue = 0;
        slider.maxValue = startingHealth;
        currentState = new States(stateWalking);

        currentState();
    }

    void Update()
    {
        slider.value = currentHealth;
        if (!bossHasDied)
        {

            if (currentState == stateIdle && isCloseToPlayer())
            {
                if (Time.time > attackTime + timerStart)
                {
                    if (getDist() <= 1.4f)
                    {
                        currentState = new States(stateStomp);
                     
                    }
                    else
                    {
                        currentState = new States(stateSwing);
                       
                    }


                    timerStart = Time.time;
                    attackTime = 4f;//to make subsequent attacks take longer

                    currentState();

                }
            }
            else if (currentState == stateStomp || currentState == stateSwing || currentState == stateIdle || currentState == stateMissileFire && !isAttacking)//transition back to walking if player is no longer close
            {
                if (isCloseToPlayer())
                {
                    attackTime = 2f;
                    currentState = new States(stateIdle);

                    currentState();
                }
                else if (!isCloseToPlayer())
                {
                    attackTime = 2f;//to reset to default
                    currentState = new States(stateWalking);

                    currentState();
                }
            }
            else if (currentState == stateWalking)//to keep walking or fire a missle/spawnDrone
            {
                if (Time.time > missileTimer + missileTimerStart)
                {
                    missileTimerStart = Time.time;

                    currentState = new States(stateMissileFire);
                }

                currentState();
            }
        }


    }


    void stateIdle()//plays idle anim and stops any other actions
    {
        isAttacking = false;
        animatorReset();

        rb.velocity = moveDirection * 0f;



        if (!isCloseToPlayer())
        {
            currentState = new States(stateWalking);

            currentState();
        }
    
    }

    void stateWalking()
    {
        animatorReset();

        updateMoveDir();

        animator.SetFloat("isMoving", 1f);

        move();

        Flip();

        if (isCloseToPlayer())
        {
            currentState = new States(stateIdle);

            currentState();
        }
    }

    void stateStomp()
    {
        animatorReset();

        StartCoroutine(stomp());
    }

    IEnumerator stomp()
    {
        animator.SetBool("Stomp", true);

        isAttacking = true;
        yield return new WaitForSeconds(1.317f);

        

        yield return null;
    }

    void stateSwing()
    {
        animatorReset();

        StartCoroutine(swing());
    }

    IEnumerator swing()
    {
        animator.SetBool("Swing", true);


        isAttacking = true;
        yield return new WaitForSeconds(2.1f);

        

        yield return null;
    }

    void stateHitstun()//can be staggered from movement patterns
    {
        animatorReset();
        animator.SetBool("hasTakenDmg", true);

        Invoke("setStatetoIdle", hitStunTime);//how long he is stunned before returning to idle
        animatorReset();
    }

    void stateCriticalHitstun()//can be staggered from anything
    {
        animatorReset();
        animator.SetBool("hasTakenCrit", true);


        Invoke("animatorReset", 2f);//return to idle animation not state//2 = hitstun anim runtime
        Invoke("setStatetoIdle", criticalHitStunTime);//how long he is stunned before returning to idle
    }

    void stateMissileFire()
    {
        animatorReset();

        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()//if there is room, spawns a drone of the right kind otherwise fires a missile and makes room for more drones
    {
        animator.SetBool("ShootMissile", true);

        rb.velocity = moveDirection * 0f;

        yield return new WaitForSeconds(1.367f);

        if(flyingDroneCount < 2)
        {
            Factory(flyingDrone, spawnpoint.transform.position, spawnpoint.transform.rotation, controller);
            flyingDroneCount++;
        }
        else if (groundDroneCount < 1)
        {
            Factory(groundDrone, spawnpoint.transform.position, spawnpoint.transform.rotation, controller);
            groundDroneCount++;
        }
        else
        {
            //fire missile
            groundDroneCount = 0f;
            flyingDroneCount = 0f;
        }

        yield return null;
    }

    void stateDeath()
    {
        animator.SetBool("hasDied", true);



        animatorReset();

    }

    //
    //nonStateFunctions:
    //

   

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
        if (currentState == stateWalking)
        {
            rb.velocity = moveDirection * moveSpeed;
        }

    }

    void animatorReset()
    {
        animator.SetFloat("isMoving", 0f);
        animator.SetBool("Swing", false);
        animator.SetBool("Stomp", false);
        animator.SetBool("ShootMissile", false);
        animator.SetBool("hasTakenDmg", false);
        animator.SetBool("hasTakenCrit", false);
    }

    void onDeath()
    {
        bossHasDied = true;

        currentState = new States(stateDeath);

        currentState();
    }

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        if (currentHealth <= 0)
        {
            return false;
        }

        base.ProcessDamage(Source, Value, EventInfo, Instigator);

        currentHealth -= Value;

        if (currentHealth <= 0)
        {
            onDeath();

            return false;
        }

        Actor tookCrit = Source.GetComponent<BossEyeHitbox>();

        if (tookCrit)
        {
            currentState = new States(stateHitstun);

            currentState();
        }

        tookCrit = Source.GetComponent<BossTopHitbox>();

        if (tookCrit)
        {
            currentState = new States(stateCriticalHitstun);

            currentState();
        }


        return true;
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

    void setStatetoIdle()
    {
        currentState = new States(stateIdle);
    }

}

