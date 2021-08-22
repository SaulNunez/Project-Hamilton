using Extensions;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sequence puzzle config
/// 
/// Currently messy, there's no way to make sure on server this has been completed correctly.
/// </summary>
public class SequencePuzzle : NetworkBehaviour
{
    [Tooltip("Puzzle we are controlling")]
    [SerializeField]
    SequenceGrid sequencePuzzle;
    
    [Tooltip("Sequence puzzle to mark as solved")]
    [SerializeField]
    PuzzleId sequenceId;

    /// <summary>
    /// Call after sequence has been completed to count it on server
    /// </summary>
    [Client]
    public void SetPuzzleComplete()
    {
        CmdCompletePuzzle();
    }

    [Command(requiresAuthority = false)]
    void CmdCompletePuzzle(NetworkConnectionToClient sender = null)
    {
        this.SuperPrint("Sequence puzzle completed");
        PuzzleCompletion.instance.MarkCompleted(sequenceId, sender.identity);
        TargetClosePuzzle(sender);
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
