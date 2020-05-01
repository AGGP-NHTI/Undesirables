using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTopHitbox : Actor
{
    public GameObject Boss;
    float damageAmount = 150f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HammerDamage player = other.gameObject.GetComponent<HammerDamage>();

        if (player)
        {
            Boss.GetComponent<BossPawn>().TakeDamage(this, damageAmount, null, Owner);

        }
    }
}
