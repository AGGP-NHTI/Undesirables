using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneAttackScript : MonoBehaviour
{

    FlyingPawn parent;

    private void Start()
    {
        parent = GetComponentInParent<FlyingPawn>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!parent.isDead)
        {
            parent.flyAttack(collision);
        }
    }
}
