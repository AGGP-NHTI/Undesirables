using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStompAttack : Actor
{
    float damageAmount = 25f;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        HeroPawn OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (OtherActor)
        {            
            OtherActor.TakeDamage(this, damageAmount, new DamageEventInfo(), Owner);
        }
    }
}
