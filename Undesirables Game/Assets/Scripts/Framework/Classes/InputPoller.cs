using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

/// <summary>
/// This class manages player input on a per player basis. 
/// This class is used by the PlayerControler class to provide Input information. 
/// Inherit this class to define player inputs. Support for all 16 players in Unity Supported. 
/// An Example for Player 1 is provided in this class. 
/// </summary>
public class InputPoller : Info
{

    private const int MAXPLAYERS = 4;
    private Gamepad[] players = new Gamepad[MAXPLAYERS];

    public bool P1UsesKB = false;
    public bool P2UsesKB = false;
    public bool AllowJoins = true;

    protected static InputPoller _Self;
    /// <summary>
    /// Public Interface to Applications's Single Reference of this class. 
    /// </summary>
    public static InputPoller Self
    {
        get { return _Self; }
    }

    /// <summary>
    /// Initalizes the Sington Reference. 
    /// </summary>
    private void Awake()
    {
        if (_Self)
        {
            Debug.LogError("Multiple Input Poller Classes Exist. This is a singleton object and only one should exist EVER.");
            return;
        }
        _Self = this;

    }

    public void Update()
    {
        if (AllowJoins)
        {
            FindJoiningPlayers();
        }
    }

    public void FindJoiningPlayers()
    {
        Gamepad[] connectedPads = Gamepad.all.ToArray();
        //LOG("Pads: " + connectedPads.Length);
        foreach (Gamepad pad in connectedPads)
        {



            if (pad.rightStickButton.wasPressedThisFrame)
            {
                LOG("Found Device: " + pad.description.ToString());
                JoinUnjoin(pad);
            }
        }
    }

    public void JoinUnjoin(Gamepad device)
    {
        int index = 0;
        int foundIndex = -1;

        bool isInlist = false;
        foreach (Gamepad pad in players)
        {
            if (pad != null)
            {
                if (pad.deviceId == device.deviceId)
                {
                    isInlist = true;
                    foundIndex = index;
                }
            }
            index++;
        }

        if (isInlist)
        {
            players[foundIndex] = null;
            return;
        }

        index = 0;
        bool islooking = true;
        while (islooking)
        {
            if (players[index] == null)
            {
                players[index] = device;
                islooking = false;
            }
            index++;

            if (index >= MAXPLAYERS)
            {
                islooking = false;
                LOG_ERROR("Can't Connect More players");
            }
        }

    }

    public virtual InputState GetPlayerInput(int playerNumber)
    {

        InputState IS = InputState.GetBlankState();

        if (P1UsesKB && (playerNumber == 0)) { GetKeyboard1Input(IS); return IS; }
        if (P2UsesKB && (playerNumber == 1)) { GetKeyboard2Input(IS); return IS; }


        if (playerNumber >= MAXPLAYERS)
        {
            // Error Check... What the heck player did you give me?
            LOG_ERROR("Asking for player " + playerNumber + " when max is" + MAXPLAYERS);
            return IS;
        }

        if (players[playerNumber] != null)
        {
            GamepadInput(IS, players[playerNumber]);
        }

        return IS;
    }



    public virtual bool IsGamepadConnected(Gamepad device)
    {
        bool result = false;

        Gamepad[] connectedPads = Gamepad.all.ToArray();

        foreach (Gamepad pad in connectedPads)
        {
            if (pad.deviceId == device.deviceId)
            {
                result = true;
            }
        }
        return result;
    }

    public virtual void GamepadInput(InputState IS, Gamepad device)
    {
        if (!device.added)
        {
            return;
        }

        // Example Input binding. 
        IS.AddAxis("Horizontal", device.leftStick.x.ReadValue());
        IS.AddAxis("Vertical", device.leftStick.y.ReadValue());
        IS.AddButton("Fire1", device.buttonSouth.wasPressedThisFrame);
        IS.AddButton("Fire2", device.buttonWest.wasPressedThisFrame);
        IS.AddButton("Fire3", device.buttonEast.wasPressedThisFrame);
        IS.AddButton("Fire4", device.buttonNorth.wasPressedThisFrame);

    }

    /// <summary>
    /// Input Setup Method for Specific Player. A
    /// dd Implementation in Inherited Class for Application
    /// </summary>
    /// <returns>InputState for Requested Player</returns>
    public virtual void GetKeyboard1Input(InputState IS)
    {
        // Example Input binding. 

        IS.AddAxis("Horizontal", Input.GetAxis("Horizontal"));
        IS.AddAxis("Vertical", Input.GetAxis("Vertical"));
        IS.AddButton("Fire1", Input.GetButtonDown("Fire1"));
        IS.AddButton("Fire2", Input.GetButtonDown("Fire2"));
        IS.AddButton("Fire3", Input.GetButtonDown("Fire3"));
        IS.AddButton("Fire4", Input.GetButtonDown("Fire4"));

    }

    /// <summary>
    /// Input Setup Method for Specific Player. 
    /// Add Implementation in Inherited Class for Application
    /// </summary>
    /// <returns>InputState for Requested Player</returns>
    public virtual void GetKeyboard2Input(InputState IS)
    {
        // Example Input binding. 

        IS.AddAxis("Horizontal", Input.GetAxis("P2Horizontal"));
        IS.AddAxis("Vertical", Input.GetAxis("P2Vertical"));
        IS.AddButton("Fire1", Input.GetButtonDown("P2Fire1"));
        IS.AddButton("Fire2", Input.GetButtonDown("P2Fire2"));
        IS.AddButton("Fire3", Input.GetButtonDown("P2Fire3"));
        IS.AddButton("Fire4", Input.GetButtonDown("P2Fire4"));

    }


}
