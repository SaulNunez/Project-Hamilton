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
    public void SendNewGeneratorValue(float value)
    {
        CmdSendVoltage((int)value);
    }

    [Command(ignoreAuthority =true)]
    void CmdSendVoltage(int value, NetworkConnectionToClient sender = null)
    {
        print($"Check,val: {value}, expected: {expectedVoltage}");
        if(value == expectedVoltage)
        {
            SetPuzzleAsCompleted(sender);
        }
    }
}
