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

    public override void Fire1(bool value)
    {
        HeroPawn HP = ((HeroPawn)PossesedPawn);
        if (HP)
        {
            HP.Fire1(value);
        }
    }

    public override void Fire2(bool value)
    {
        HeroPawn HP = ((HeroPawn)PossesedPawn);
        if (HP)
        {
            HP.Fire2(value);
        }
    }

    public override void Fire3(bool value)
    {
        HeroPawn HP = ((HeroPawn)PossesedPawn);
        if (HP)
        {
            HP.Fire3(value);
        }
    }


}
