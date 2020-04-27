using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordAttack : Actor
{
    float damageAmount = 25f;
    public bool isLeft = true;
    public GameObject BossObj;
    BossPawn Boss;

    void Start()
    {
        Boss = BossObj.GetComponent<BossPawn>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if(OtherActor)
        {
            /*if(isLeft)
            {
                Boss.toggleHitboxes(2);
            }
            else
            {
                Boss.toggleHitboxes(3);
            }*/

            OtherActor.TakeDamage(this, damageAmount, new DamageEventInfo(), Owner);
        }
    }
}
