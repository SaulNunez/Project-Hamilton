using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IfThermometer : PuzzleBase
{
    [SerializeField]
    TMP_Text currentTemp;

    //Server values
    [SyncVar]
    int uiTemperature;

    public override void OnStartServer()
    {
        base.OnStartServer();
        uiTemperature = UnityEngine.Random.Range(0, 60);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        currentTemp.text = uiTemperature.ToString();
    }

    const int SELECT_NOTHING = 0;
    const int SELECT_PRINT_CORRENT_TEMP = 1;

    public void SelectNothing()
    {
        CmdVerify(SELECT_NOTHING);
    }

    public void SelectedPrint()
    {
        CmdVerify(SELECT_PRINT_CORRENT_TEMP);
    }

    [Command(requiresAuthority = false)]
    public void CmdVerify(int selection, NetworkConnectionToClient sender = null)
    {
        if(uiTemperature >= 32)
        {
            if(selection == SELECT_PRINT_CORRENT_TEMP)
            {
                PuzzleCompletion.instance.MarkCompleted(PuzzleId.TemperatureCompareInt, sender.identity);
                TargetClosePuzzle(sender);
                TargetRunCorrectFeedback(sender);
            }
            else
            {
                TargetRunWrongFeedback(sender);
            }
        } else
        {
            if (selection == SELECT_NOTHING)
            {
                PuzzleCompletion.instance.MarkCompleted(PuzzleId.TemperatureCompareInt, sender.identity);
                TargetClosePuzzle(sender);
                TargetRunCorrectFeedback(sender);
            }
            else
            {
                TargetRunWrongFeedback(sender);
            }
        }
    }
}
