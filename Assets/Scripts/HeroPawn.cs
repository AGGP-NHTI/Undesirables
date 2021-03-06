﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroPawn : Pawn
{
    public TMP_Text textGrenade;
    public List<GameObject> Grenades = new List<GameObject>();

    public bool isDead = false;
    public float Health = 100.0f;
    public SpriteRenderer body;
    public SpriteRenderer head;
    public SpriteRenderer upperArmLeft;
    public SpriteRenderer upperArmRight;
    public SpriteRenderer lowerArmLeft;
    public SpriteRenderer lowerArmRight;
    public SpriteRenderer upperLegLeft;
    public SpriteRenderer upperLegRight;
    public SpriteRenderer lowerLegLeft;
    public SpriteRenderer lowerLegRight;

    public GameObject grenadePrefab;
    public GameObject grenadeLeftPrefab;
    public GameObject grenadeSpawnLoc;
    public Collider2D hammerHitBox;
    Rigidbody2D rb;
    public float Speed = 50f;
    private float attackCoolDwn = 0.35f;
    private float ProjattackCoolDwn = 0.3f;
    private float hurtTime = 0.2f;
    public bool facingRight;
    private bool ismAing = false;
    private bool ispAing = false;
    private bool ispAingInAir = false;
    private bool ismAingInAir = false;
    private bool isHurt = false;
    private bool isPlayerDead;
    public bool inAir = false;
    public Vector3 theScale;
    private float jumpForce = 6.5f;
    public int numOfGrenades;
    public Canvas playerHealth;
    public Slider sliderHealth;

    void Start()
    {
        numOfGrenades = 0;
        Grenades.Add(grenadePrefab);
        numOfGrenades++;
        Grenades.Add(grenadePrefab);
        numOfGrenades++;
        isPlayerDead = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        hammerHitBox.enabled = false;
        theScale = Vector3.zero;
        facingRight = true;
        sliderHealth.maxValue = Health;
    }

    private void Update()
    {
        sliderHealth.value = Health;
        textGrenade.text = numOfGrenades.ToString();

        if (isHurt)
        {
            if (hurtTime <= 0f)
            {
                body.color = new Color(1f, 1f, 1f, 1f);
                head.color = new Color(1f, 1f, 1f, 1f);
                upperLegRight.color = new Color(1f, 1f, 1f, 1f);
                upperLegLeft.color = new Color(1f, 1f, 1f, 1f);
                upperArmRight.color = new Color(1f, 1f, 1f, 1f);
                upperArmLeft.color = new Color(1f, 1f, 1f, 1f);
                lowerLegRight.color = new Color(1f, 1f, 1f, 1f);
                lowerLegLeft.color = new Color(1f, 1f, 1f, 1f);
                lowerArmRight.color = new Color(1f, 1f, 1f, 1f);
                lowerArmLeft.color = new Color(1f, 1f, 1f, 1f);
                isHurt = false;
                hurtTime = 0.2f;
            }
            else
            {
                hurtTime = hurtTime - Time.deltaTime;
            }
        }
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
                    hammerHitBox.enabled = false;
                    ismAing = false;
                    ismAingInAir = false;
                }
                else
                {
                    attackCoolDwn = 0.35f;
                    gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
                    hammerHitBox.enabled = false;
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
                    if (facingRight)
                    {
                        Instantiate(grenadePrefab, grenadeSpawnLoc.transform.position, Quaternion.identity);
                        numOfGrenades--;
                    }
                    else
                    {
                        Instantiate(grenadeLeftPrefab, grenadeSpawnLoc.transform.position, Quaternion.identity);
                        numOfGrenades--;
                    }
                    ProjattackCoolDwn = 0.3f;
                    gameObject.GetComponent<Animator>().SetBool("ProjInAir", false);
                    ispAing = false;
                    ispAingInAir = false;
                }
                else
                {
                    if (facingRight)
                    {
                        Instantiate(grenadePrefab, grenadeSpawnLoc.transform.position, Quaternion.identity);
                        numOfGrenades--;
                    }
                    else
                    {
                        Instantiate(grenadeLeftPrefab, grenadeSpawnLoc.transform.position, Quaternion.identity);
                        numOfGrenades--;
                    }
                    ProjattackCoolDwn = 0.3f;
                    gameObject.GetComponent<Animator>().SetBool("isProjAtt", false);
                    ispAing = false;
                }
            }
        }

    }

    public virtual void Horizontal(float value)
    {
        if (isPlayerDead == false)
        {
            if (value == -1)
            {
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
    }

    public virtual void Fire1(bool value)
    {
        if (isPlayerDead == false)
        {
            if ((value) && (inAir == false))
            {
                Jump();
            }
        }
    }

    public virtual void Fire2(bool value)
    {
        if (isPlayerDead == false)
        {
            if ((value) && (ismAing == false))
            {
                mAttack();
            }
        }
    }

    public virtual void Fire3(bool value)
    {
        if (isPlayerDead == false)
        {
            if ((value) && (ispAing == false))
            {
                if (numOfGrenades >= 1)
                {
                    pAttack();
                }
            }
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
            hammerHitBox.enabled = true;
            ismAing = true;
            ismAingInAir = true;
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
            hammerHitBox.enabled = true;
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


    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        isHurt = true;
        body.color = new Color(1f, 0f, 0f, 1f);
        head.color = new Color(1f, 0f, 0f, 1f);
        upperArmLeft.color = new Color(1f, 0f, 0f, 1f);
        upperArmRight.color = new Color(1f, 0f, 0f, 1f);
        upperLegLeft.color = new Color(1f, 0f, 0f, 1f);
        upperLegRight.color = new Color(1f, 0f, 0f, 1f);
        lowerArmLeft.color = new Color(1f, 0f, 0f, 1f);
        lowerArmRight.color = new Color(1f, 0f, 0f, 1f);
        lowerLegLeft.color = new Color(1f, 0f, 0f, 1f);
        lowerLegRight.color = new Color(1f, 0f, 0f, 1f);

        LOG("Took Damage");
        Controller controller = GetController();
        Health = Health - Value;
        Debug.Log(Health);
        if (Health <= 0f)
        {

            isDead = true;
            rb.velocity = new Vector2(0f, 0f);
            isPlayerDead = true;
            gameObject.GetComponent<Animator>().SetBool("isHeroDead", true);
            //Destroy(gameObject, 5f);

            Debug.Log(gameObject.name + " has died!");
            IgnoresDamage = true;
            //Game.Self.PlayerDied(this, Source, EventInfo, Instigator);
            //Debug.Log(gameObject.name + " was killed by " + Instigator.playerName + " ripripripripripripripriprip");
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

        return false;
    }
}
