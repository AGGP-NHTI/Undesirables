using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneAttackScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlyingPawn parent = GetComponentInParent<FlyingPawn>();
        parent.flyAttack(collision);
    }
}
