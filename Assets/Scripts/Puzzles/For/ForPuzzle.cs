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
    [SyncVar(hook = nameof(OnTurnsToWashSet))]
    int turnsToWash;

    [SerializeField]
    TextMeshProUGUI instructionTextbox;

    [Tooltip("Instructions to solve puzzle. Will be passed to string.format for setting variable turns. Use {0} to interpolate turnsToWash.")]
    [TextArea]
    [SerializeField]
    public string instructionText;

    [Tooltip("Input field to input the expected amount needed to wash")]
    [SerializeField]
    TMP_InputField input;

    /// <summary>
    /// Called on turnsToWash updated on client (called when screen is started). Updates instruction box to on server decided turnsToWash.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="newValue"></param>
    void OnTurnsToWashSet(int _, int newValue)
    {
        instructionTextbox.text = string.Format(instructionText, newValue);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        turnsToWash = Random.Range(5, 45);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        input.onEndEdit.AddListener(CheckResultIsCorrect);
    }

    /// <summary>
    /// Exposed to be added on editor to OnEndEdit on the InputField. Takes input and sends it to the server to be checked to be correct.
    /// </summary>
    /// <param name="result">Textbox content</param>
    [Client]
    public void CheckResultIsCorrect(string result)
    {
        try
        {
            var turnsInput = int.Parse(result);
            CmdCheckInputMatchesTurns(turnsInput);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Checks client input to match turnsToWash.
    /// </summary>
    /// <param name="input"></param>
    [Command(ignoreAuthority = true)]
    void CmdCheckInputMatchesTurns(int input, NetworkConnectionToClient sender = null)
    {
        if(input == turnsToWash)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.ForWashingBucket);
            RpcClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void RpcClosePuzzle(NetworkConnection target)
    {
        ShowPuzzle.instance.ClosePuzzles();
    }
}
