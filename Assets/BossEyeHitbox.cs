using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyeHitbox : Actor
{
    public GameObject Boss;
    float damageAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HeroPawn player = other.gameObject.GetComponent<HeroPawn>();

        if(player)
        {
            Boss.GetComponent<BossPawn>().TakeDamage(this, damageAmount, null, Owner);
            Debug.Log(Owner);
        }
    }
}