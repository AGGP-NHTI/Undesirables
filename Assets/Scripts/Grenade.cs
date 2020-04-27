using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Actor
{

    public float damageAmount = 10.0f;
    public float movementSpeed = 5f;
    public float lifetime = 2f;
    private bool isRight;
    Rigidbody2D rb;

    void Start()
    {
        
        
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.velocity = (Vector2.up * movementSpeed) + (Vector2.right * movementSpeed);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, new DamageEventInfo(typeof(BaseDamageType)), Owner);
        }
        OnDeath();
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
