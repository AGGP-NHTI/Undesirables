using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDamage : Pawn
{
    private float damageAmount = 100f;
    public Collider2D hammerHitBox;
    public GameObject sparksLoc;
    public GameObject sparks;
    public AudioSource audio;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audio.Play();
        Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
        if (OtherActor)
        {
            Instantiate(sparks, sparksLoc.transform.position, Quaternion.identity);
            OtherActor.TakeDamage(this, damageAmount, null, Owner);
            hammerHitBox.enabled = false;
            
        }
    }
}
