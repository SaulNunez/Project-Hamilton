using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for do while puzzle
/// </summary>
public class DoWhilePuzzle : NetworkBehaviour
{
    /// <summary>
    /// To be used in the UI in the client to check input in server
    /// </summary>
    /// <param name="text">InputField current text</param>
    [Client]
    public void CheckTextboxInput(string text)
    {
        CmdCheckResultInServer(text);
    }

    [Command(ignoreAuthority = true)]
    void CmdCheckResultInServer(string text, NetworkConnectionToClient sender = null)
    {
        var textClean = text.Trim().ToLower();

        if (textClean == "verdadero" || textClean == "true")
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.DoWhileMotorStarter);
            TargetClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        ShowPuzzle.instance.ClosePuzzles();
    }
}
