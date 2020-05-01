using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyingStates { IDLE, MOVING, GUN, BLADEF, BLADER, DAMAGED, DEAD }

public class FlyingPawn : DronePawn
{
    public GameObject projectile;
    public GameObject spawnPoint;
    public GameObject lowpoint;
    public GameObject highpoint;
    public GameObject hitbox;
    public BoxCollider2D colbox;
    Rigidbody2D rb;
    FlyingStates states;
    public float flyHeight;
    float timer = 5.0f;
    int nextState;
    bool isDead;
    public Animator flyAn;
    public GameObject ground;
    bool facingLeft;
    bool inAttack;
    bool isFalling;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody2D>();
        states = FlyingStates.IDLE;
        isDead = false;
        facingLeft = true;
        isFalling = true;
        flyHeight = Random.Range(.75f, 6.0f);
        colbox.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (!isDead)
        {
            if (states == FlyingStates.MOVING)
            {
                rb.velocity = new Vector2(0.5f, rb.velocity.y);
            }
            if (states == FlyingStates.GUN)
            {

            }
            if (states == FlyingStates.BLADEF)
            {
                rb.velocity = new Vector2(-0.5f, -2f);
                if (gameObject.transform.position.y < lowpoint.transform.position.y)
                {
                    states = FlyingStates.BLADER;

                }
            }
            if (states == FlyingStates.BLADER)
            {
                rb.velocity = new Vector2(-0.5f, 2f);

                if (gameObject.transform.position.y > lowpoint.transform.position.y)
                {
                    states = FlyingStates.IDLE;
                }
            }
            if (transform.position.y - ground.transform.position.y < flyHeight && isFalling)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.5f);
            }

            if (timer < 0 && inAttack != true)
            {
                if (states == FlyingStates.GUN)
                {
                    timer = 5.0f;
                    flyAn.SetInteger("MoveState", 0);
                    Debug.Log("Changing States");
                    states = FlyingStates.IDLE;
                }
                else if (states == FlyingStates.IDLE)
                {
                    timer = 5.0f;
                    flyAn.SetInteger("MoveState", 1);
                    Debug.Log("Changing States");
                    states = FlyingStates.MOVING;
                    Vector2 theScale = transform.localScale;
                    theScale.x *= -1;
                    facingLeft = !facingLeft;
                    transform.localScale = theScale;
                }
                else if (states == FlyingStates.MOVING)
	            {
                    timer = 3.0f;
                    StartCoroutine(fire());
                }
            }
        }
        
    }
    IEnumerator fire()
    {
        states = FlyingStates.GUN;
        inAttack = true;
        flyAn.SetTrigger("GunAttack");
        yield return new WaitForSeconds(0.30f);
        for (int i = 0; i < 2; i++)
        {
            Factory(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation, controller);
            yield return new WaitForSeconds(0.10f);
        }
        yield return new WaitForSeconds(0.30f);
        states = FlyingStates.IDLE;
        flyAn.ResetTrigger("GunAttack");
        inAttack = false;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !inAttack)
        {
            if (collision.transform.position.x > gameObject.transform.position.x)
            {
                Vector2 theScale = transform.localScale;
                theScale.x *= -1;
                facingLeft = !facingLeft;
                transform.localScale = theScale;
            }
            inAttack = true;
            states = FlyingStates.BLADEF;
            StartCoroutine(melee());
        }
    }

    IEnumerator melee()
    {
        Debug.Log("Starting Coroutine");
        isFalling = false;
        flyAn.SetTrigger("BladeAttack");
        yield return new WaitForSeconds(0.45f);
        colbox.enabled = false;
        hitbox.SetActive(true);
        yield return new WaitForSeconds(1f);
        hitbox.SetActive(false);
        colbox.enabled = true;
        yield return new WaitForSeconds(0.30f);
        inAttack = false;
        isFalling = true;
        Debug.Log("Ending Coroutine");
        yield return null;
    }

    IEnumerator deathAn()
    {
        flyAn.SetTrigger("IsDying");
        isFalling = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo = null, Controller Instigator = null)
    {

        health -= Value;
        if (health <= 0)
        {
            IgnoresDamage = true;
            if (_controller)
            {
                _controller.RequestSpectate();
            }
            isDead = true;
            StartCoroutine(deathAn());
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
