using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor
{

    public float damageAmount;
    public float movementSpeed;
    public float lifetime;
    public Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        rb.velocity = new Vector2(-movementSpeed, 0.0f);
    }



    void OnTriggerEnter2D (Collider2D other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
        if (other.CompareTag("Player"))
        {
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
