using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePawn : Pawn
{
    public float health = 200f;

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo = null, Controller Instigator = null)
    {

        health -= Value;
        if (health <= 0)
        {
            if (_controller)
            {
                _controller.RequestSpectate();
            }
            Debug.Log(gameObject.name + " was killed by " + Instigator.playerName + " ripripripripripripripriprip");
        }

        string DamageEventString = Source.ActorName + " " + EventInfo.DamageType.verb + " " + this.ActorName + " (" + Value.ToString() + " damage)";
        if (Instigator)
        {
            DamageEventString = Instigator.playerName + " via " + DamageEventString;
        }
        else
        {
            DamageEventString = "The World via " + DamageEventString;
        }
        DAMAGELOG(DamageEventString);


        return true;
    }


}
