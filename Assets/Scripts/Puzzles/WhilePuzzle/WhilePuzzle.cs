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

    [Command(ignoreAuthority = true)]
    void CmdCheckInput(int input, NetworkConnectionToClient sender = null)
    {
        if(input == defaultValue)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.WhileFillingBucket);
            RpcClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void RpcClosePuzzle(NetworkConnection target)
    {
        ShowPuzzle.instance.ClosePuzzles();
    }
}
