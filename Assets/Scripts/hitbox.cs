using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : Actor
{
    public float damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HeroPawn OtherActor = collision.gameObject.GetComponentInParent<HeroPawn>();
        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, null, Owner);
        }
    }
}
