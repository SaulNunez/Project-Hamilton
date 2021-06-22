using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// Setup for for puzzle
/// A number is defined on start.
/// On puzzle used, there's a convertion in client to a number and then sent to the server for checking, 
/// if correct, it's registered as a done puzzle and closes puzzle for player.
/// </summary>
public class ForPuzzle : NetworkBehaviour
{
    [SyncVar]
    int turnsToWash;

    [Tooltip("Input field to input the expected amount needed to wash")]
    [SerializeField]
    TMP_InputField input;

    int turnsInClient;

    public void CountOneMoreTurn()
    {
        turnsInClient++;

        if (turnsInClient == turnsToWash)
        {
            CmdFinishPuzzle();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        turnsToWash = Random.Range(5, 45);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        input.text = turnsToWash.ToString();
    }


    /// <summary>
    /// Checks client input to match turnsToWash.
    /// </summary>
    /// <param name="input"></param>
    [Command(ignoreAuthority = true)]
    void CmdFinishPuzzle(NetworkConnectionToClient sender = null)
    {
        PuzzleCompletion.instance.MarkCompleted(PuzzleId.ForWashingBucket, sender.identity);
        RpcClosePuzzle(sender);
    }

    [TargetRpc]
    void RpcClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
