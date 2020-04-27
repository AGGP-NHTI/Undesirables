using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStompAttack : Actor
{
    float damageAmount = 25f;
    public GameObject BossObj;
    BossPawn Boss;

    void Start()
    {
        Boss = BossObj.GetComponent<BossPawn>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (OtherActor)
        {
            //Boss.toggleHitboxes(1);

            OtherActor.TakeDamage(this, damageAmount, new DamageEventInfo(), Owner);
        }
    }
}
