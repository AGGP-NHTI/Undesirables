using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDamage : HeroPawn
{
    public float damageAmount = 10.0f;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, null, Owner);
            
        }
    }
}
