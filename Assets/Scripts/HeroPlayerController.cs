using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroPlayerController : PlayerController
{

    public override void Horizontal(float value)
    {
        HeroPawn HP = ((HeroPawn)PossesedPawn);
        if (HP)
        {
            HP.Horizontal(value);
        }
    }


}
