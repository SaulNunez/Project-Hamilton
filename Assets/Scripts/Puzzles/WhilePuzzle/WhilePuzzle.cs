using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhilePuzzle : NetworkBehaviour
{
    public const int defaultValue = 6;

    public void CheckInput(string input)
    {
        try
        {
            var turnsInput = int.Parse(input);
            CmdCheckInput(turnsInput);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    [Command]
    void CmdCheckInput(int input)
    {
        if(input == defaultValue)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.WhileFillingBucket);
            RpcClosePuzzle();
        }
    }

    [ClientRpc]
    void RpcClosePuzzle()
    {
        gameObject.SetActive(false);
    }
}
