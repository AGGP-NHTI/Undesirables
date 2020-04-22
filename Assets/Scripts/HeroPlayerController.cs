using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroPlayerController : PlayerController
{
    public GameObject StartingPawn; 

    protected override void Start()
    {
        base.Start(); 

        if (StartingPawn)
        {
            PossesPawn(StartingPawn); 
        }

    }

    public override void Horizontal(float value)
    {
        HeroPawn HP = ((HeroPawn)PossesedPawn);
        if (HP)
        {
            HP.Horizontal(value);
        }
    }


}
