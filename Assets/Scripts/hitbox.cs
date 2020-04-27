using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : Actor
{
    public float damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Actor OtherActor = collision.gameObject.GetComponentInParent<Actor>();
        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, null, Owner);
        }
    }
}
