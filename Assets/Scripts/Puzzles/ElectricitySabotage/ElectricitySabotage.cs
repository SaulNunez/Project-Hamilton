using Extensions;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Puzzle for electricity sabotage
/// </summary>
public class ElectricitySabotage : SabotagePuzzle
{
    [Range(2, 4)]
    [SerializeField]
    int expectedPlayers;

    int usersSolvedPuzzle = 0;

    [SerializeField]
    int expectedVoltage = 120;

    [SerializeField]
    Slider voltageSlider;

    public override void OnStartClient()
    {
        base.OnStartClient();
        voltageSlider.onValueChanged.AddListener(SendNewGeneratorValue);
    }

    protected override bool AreEmergencyConditionsEnough(Emergency.EmergencyType type) => 
        type == Emergency.EmergencyType.TurnDownGenerator;

    /// <summary>
    /// To be called by UI on update of value of voltage to be checked if it's the one needed.
    /// </summary>
    /// <param name="value">Valor actual de UI</param>
    [Client]
    public void SendNewGeneratorValue(float value)
    {
        CmdSendVoltage((int)value);
    }

    protected override bool ArePuzzleCompletionConditionsReached()
    {
        return usersSolvedPuzzle >= expectedPlayers;
    }

    [Command(ignoreAuthority =true)]
    void CmdSendVoltage(int value, NetworkConnectionToClient sender = null)
    {
        if(value == expectedVoltage)
        {
            usersSolvedPuzzle++;
            this.SuperPrint($"{sender.address}: Player has finished electricity puzzle");
            SetPuzzleAsCompleted(sender);
        }
    }

    protected override void ResetSabotage(int _)
    {
        base.ResetSabotage(_);

        usersSolvedPuzzle = 0;
    }
}
