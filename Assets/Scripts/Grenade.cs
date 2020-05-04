using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Actor
{

    public float damageAmount = 50.0f;
    public float movementSpeed = 5f;
    public float lifetime = 2f;
    public Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();        
        rb.velocity = (Vector2.up * movementSpeed) + (Vector2.right * movementSpeed);
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
            if (OtherActor)
            {
                OtherActor.TakeDamage(this, damageAmount, null, Owner);
            }
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
