using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for do while puzzle
/// </summary>
public class DoWhilePuzzle : NetworkBehaviour
{
    public void CheckTextboxInput(string text)
    {
        CmdCheckResultInServer(text);
    }

    [Command]
    void CmdCheckResultInServer(string text)
    {
        if(text.ToLower() == "verdadero")
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.DoWhileMotorStarter);
            RpcClosePuzzle();
        }
    }

    [ClientRpc]
    void RpcClosePuzzle()
    {
        gameObject.SetActive(false);
    }
}
