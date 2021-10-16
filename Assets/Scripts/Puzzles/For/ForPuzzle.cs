using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Serialization;

/// <summary>
/// Setup for for puzzle
/// A number is defined on start.
/// On puzzle used, there's a convertion in client to a number and then sent to the server for checking, 
/// if correct, it's registered as a done puzzle and closes puzzle for player.
/// </summary>
public class ForPuzzle : PuzzleBase
{
    [SyncVar]
    int turnsToWash;

    [Tooltip("Input field to input the expected amount needed to wash")]
    [SerializeField]
    [FormerlySerializedAs("input")]
    TMP_InputField topTurnText;

    [SerializeField]
    TMP_InputField turnsInput;

    int currentPlayerInput;

    public override void OnStartServer()
    {
        base.OnStartServer();

        turnsToWash = Random.Range(5, 45);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        topTurnText.text = turnsToWash.ToString();
        turnsInput.onEndEdit.AddListener(SaveNewInputValue);
    }

    [Client]
    void SaveNewInputValue(string value)
    {
        try
        {
            currentPlayerInput = int.Parse(value);
        } catch (Exception)
        {

        }
    }

    [Client]
    public void Verify()
    {
        CmdFinishPuzzle(currentPlayerInput);
    }

    /// <summary>
    /// Checks client input to match turnsToWash.
    /// </summary>
    /// <param name="input"></param>
    [Command(requiresAuthority = false)]
    void CmdFinishPuzzle(int playerInput, NetworkConnectionToClient sender = null)
    {
        if(playerInput == turnsToWash + 1)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.ForWashingBucket, sender.identity);
            RpcClosePuzzle(sender); 
            TargetRunCorrectFeedback(sender);
        }
        else
        {
            TargetRunWrongFeedback(sender);
        }
    }

    [TargetRpc]
    void RpcClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
