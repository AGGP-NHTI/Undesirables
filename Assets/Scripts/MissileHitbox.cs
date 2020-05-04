using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHitbox : Actor
{
    public GameObject mainMissile;
    float damageAmount = 50f;

    void Start()
    {
        Destroy(mainMissile, 3f);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        HeroPawn OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, new DamageEventInfo(), Owner);

            Destroy(mainMissile);
        }
    }

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        
        base.ProcessDamage(Source, Value, EventInfo, Instigator);

        if (Value > 0)
        {
            Destroy(mainMissile);
        }

        return true;
    }
}