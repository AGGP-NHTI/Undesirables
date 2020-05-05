using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHitbox : Actor
{
    public GameObject mainMissile;
    public GameObject explosion;
    float damageAmount = 50f;

    void Start()
    {
        Invoke("OnDeath", 3f); 
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        HeroPawn OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, new DamageEventInfo(), Owner);

            OnDeath();
        }
        else if (other.gameObject.GetComponent<Grenade>() || other.gameObject.GetComponent<GrenadeLeft>())
        {
            if(other.gameObject.GetComponent<Grenade>())
            {
                other.gameObject.GetComponent<Grenade>().OnDeath();
            }
            else
            {
                other.gameObject.GetComponent<GrenadeLeft>().OnDeath();
            }
            OnDeath();
        }
        else if(other.gameObject.GetComponent<HammerDamage>())
        {
            OnDeath();
        }
    }

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        
        base.ProcessDamage(Source, Value, EventInfo, Instigator);

        if (Value > 0)
        {
            OnDeath();
        }

        return true;
    }

    void OnDeath()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(mainMissile);
    }
}