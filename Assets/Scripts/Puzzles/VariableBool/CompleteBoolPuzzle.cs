using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteBoolPuzzle : NetworkBehaviour
{
    public void CompletePuzzle(bool toggleState)
    {
        CmdSendPuzzleState(toggleState);
    }

    [Command(ignoreAuthority = true)]
    void CmdSendPuzzleState(bool toggleState, NetworkConnectionToClient sender = null)
    {
        if (toggleState)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.VariableBoolean);
            TargetClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        ShowPuzzle.instance.ClosePuzzles();
    }
}
