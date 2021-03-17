using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Puzzle for electricity sabotage
/// </summary>
public class ElectricitySabotage : SabotagePuzzle
{
    [SerializeField]
    int expectedVoltage = 120;

    protected override void OnPuzzleCompleted()
    {
        base.OnPuzzleCompleted();
        //TODO: Create code to turn off emergency
    }

    /// <summary>
    /// To be called by UI on update of value of voltage to be checked if it's the one needed.
    /// </summary>
    /// <param name="value">Valor actual de UI</param>
    [Client]
    public void SendNewGeneratorValue(int value)
    {
        CmdSendVoltage(value);
    }

    [Command(ignoreAuthority =true)]
    void CmdSendVoltage(int value, NetworkConnectionToClient sender = null)
    {
        if(value == expectedVoltage)
        {
            SetPuzzleAsCompleted(sender);
            ClosePuzzle();
        }
    }
}
