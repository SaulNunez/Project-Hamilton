using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Logic for do while puzzle
/// </summary>
public class DoWhilePuzzle : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("The input field used to input the boolean that solves this question")]
    TMP_InputField input;

    public override void OnStartClient()
    {
        base.OnStartClient();

        input.onEndEdit.AddListener(CheckTextboxInput);
    }

    /// <summary>
    /// To be used in the UI in the client to check input in server
    /// </summary>
    /// <param name="text">InputField current text</param>
    [Client]
    public void CheckTextboxInput(string text)
    {
        print($"Sending to server: {text}");
        CmdCheckResultInServer(text);
    }

    [Command(ignoreAuthority = true)]
    void CmdCheckResultInServer(string text, NetworkConnectionToClient sender = null)
    {
        var textClean = text.Trim().ToLower();

        if (textClean == "verdadero" || textClean == "true")
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.DoWhileMotorStarter, sender.identity);
            TargetClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
