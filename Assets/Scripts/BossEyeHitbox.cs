using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyeHitbox : Actor
{
    public GameObject Boss;
    float damageAmount = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HammerDamage player = other.gameObject.GetComponent<HammerDamage>();

        if(player)
        {
            Boss.GetComponent<BossPawn>().TakeDamage(this, damageAmount, null, Owner);
            
        }
    }
}