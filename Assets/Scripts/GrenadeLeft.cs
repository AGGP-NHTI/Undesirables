using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLeft : Actor
{

    public float damageAmount = 50.0f;
    public float movementSpeed = 5f;
    public float lifetime = 2f;
    public Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = (Vector2.up * movementSpeed) + (Vector2.left * movementSpeed);
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.tag != "Player") && (other.gameObject.tag != "ground"))
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
