using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPawn : Pawn
{
    delegate void States();

    States currentState;

    Rigidbody2D rb;

    public Animator animator;
    public GameObject player;
    public GameObject footHitboxObj;
    public GameObject leftSwordHitboxObj;
    public GameObject rightSwordHitboxObj;
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
    bool nextAttack = true;//to decide what melee attack is used.
    bool isAttacking = false;
    Vector3 theScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        footHitbox = footHitboxObj.GetComponent<Collider2D>();
        leftSwordHitbox = leftSwordHitboxObj.GetComponent<Collider2D>();
        rightSwordHitbox = rightSwordHitboxObj.GetComponent<Collider2D>();

        currentState = new States(stateWalking);

        currentState();
    }

    void Update()
    {
        if (!bossHasDied)
        {

            if (currentState == stateIdle && isCloseToPlayer() && attackTime > 0)
            {
                if (Time.time > attackTime + timerStart)
                {
                    if (getDist() <= 1.4f)
                    {
                        currentState = new States(stateStomp);

                        nextAttack = false;
                    }
                    else
                    {
                        currentState = new States(stateSwing);

                        nextAttack = true;
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

        toggleHitboxes(1);

        isAttacking = true;
        yield return new WaitForSeconds(1.317f);

        toggleHitboxes(1);
        isAttacking = false;

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

        toggleHitboxes(2);

        isAttacking = true;
        yield return new WaitForSeconds(2.1f);

        toggleHitboxes(2);
        isAttacking = false;

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

    public void toggleHitboxes(int which = 0)//also disables any active hitboxes that shouldn't be there
    {
        if (which == 1 || footHitbox.enabled)
        {
            footHitbox.enabled = !footHitbox.enabled;
        }
        else if (which == 2 || leftSwordHitbox.enabled)
        {
            leftSwordHitbox.enabled = !leftSwordHitbox.enabled;
        }
        else if (which == 3 || rightSwordHitbox.enabled)
        {
            rightSwordHitbox.enabled = !rightSwordHitbox.enabled;
        }
        else
        {
            Debug.Log("BossPawn: Invalid input recived in toggleHitboxes");
        }
    }

    public void toggleHitboxes2()//to be called via Invoke to disabe any active hitboxes
    {
        if (footHitbox.enabled)
        {
            footHitbox.enabled = !footHitbox.enabled;
        }
        else if (leftSwordHitbox.enabled)
        {
            leftSwordHitbox.enabled = !leftSwordHitbox.enabled;
        }
        else if (rightSwordHitbox.enabled)
        {
            rightSwordHitbox.enabled = !rightSwordHitbox.enabled;
        }
       
    }
}

