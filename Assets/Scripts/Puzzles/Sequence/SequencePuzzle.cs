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

    [Command(ignoreAuthority =true)]
    void CmdCompletePuzzle(NetworkConnectionToClient sender = null)
    {
        PuzzleCompletion.instance.MarkCompleted(sequenceId);
        TargetClosePuzzle(sender);
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
