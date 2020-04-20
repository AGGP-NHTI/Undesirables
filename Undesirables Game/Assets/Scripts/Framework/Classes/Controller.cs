﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Info {

    public bool IsAI = false;
    public bool IsHuman = false;
    public bool UseSpawnSystem = true;
    public string playerName;

    public static bool LogPossessionFailures = true;
    public bool StartWithSpectator = true; 
    public GameObject SpawnPreFab;
    public GameObject SpectatorPreFab;
    public GameObject SpectatorActor;
    //protected Pawn SpeactatorPawn; 
    
    /// <summary>
    /// Player Number for Grabbing Input
    /// </summary>
    public int InputPlayerNumber = 0;

    /// <summary>
    /// Player Number in the game
    /// </summary>
    public int PlayerNumber = 0; 

    // We'll enum this later 
    public int PlayerType = 1;



    protected Pawn PossesedPawn;

    protected virtual void Start()
    {
        if (!UseSpawnSystem)
        {
            return; 
        }

        // Create Spectator Prefab
        if (SpectatorPreFab)
        {
            SpectatorActor = Factory(SpectatorPreFab, Vector3.zero, Quaternion.identity, this); 
        }

        if (StartWithSpectator)
        {
            // Grab the Spectator Prefab
            RequestSpectate(); 
        }
        else
        {
            //  Spawn and new Active Game Object and grab it. 
            RequestSpawn();  
        }
    }

    public Pawn GetPossesedPawn()
    {
        return PossesedPawn; 
    }

    public virtual bool PossesPawn(Pawn p)
    {
        
        if (!(p.Possesed(this)))
         {
            LOG_ERROR("Controler - Possession Failure"); 
            return false; 
        }

        // If we have a Possessed Pawn already, Call Unpossess on it. 
        if (PossesedPawn)
        {
            PossesedPawn.BecomeUnPossesed();
        }

        PossesedPawn = p; 
        return true; 
    }

    /// <summary>
    /// PossesPawn version taking GameObject with Pawn Script attached to it. 
    /// </summary>
    /// <param name="PawnGameObject">Game Object with Pawn Script Attached to it</param>
    /// <returns></returns>
    public virtual bool PossesPawn(GameObject PawnGameObject)
    {
        Pawn p = PawnGameObject.GetComponent<Pawn>(); 
        if (!p)
        {
            LOG_ERROR("GameObject " + PawnGameObject.name + " is not a pawn.");
            return false; 
        }
        return PossesPawn(p);
    }

    public virtual bool UnPossesPawn(Pawn p)
    {
        p.BecomeUnPossesed(); 

        PossesedPawn = null;
        return true;
    }

    public GameObject RequestSpawn()
    {
        if (!SpawnPreFab)
        {
            LOG_ERROR("No Spawn Prefab Set for Spawning");
            return null;
        }
        return Game.Self.RequestSpawn(this, SpawnPreFab);
    }

    public bool RequestSpectate()
    {
        if (!SpectatorActor)
        {
            LOG_ERROR("*** No Spectator Actor Assigned");
            return false; 
        }
        PossesPawn(SpectatorActor);
        return true;
    }

}
