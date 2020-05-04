using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDamage : Pawn
{
    private float damageAmount = 100f;
    public Collider2D hammerHitBox;
    

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
        if (OtherActor)
        {
            OtherActor.TakeDamage(this, damageAmount, null, Owner);
            hammerHitBox.enabled = false;
            
        }
    }
}
