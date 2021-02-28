using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCompletion : NetworkBehaviour
{
    /// <summary>
    /// Called on puzzle completion.
    /// </summary>
    /// <remarks>
    /// <strong>Only applicable on the server</strong>
    /// </remarks>
    public static event Action<PuzzleId> OnPuzzleCompleted;

    public SyncList<PuzzleId> puzzlesCompleted = new SyncList<PuzzleId>();

    //private void Start()
    //{
    //    puzzlesCompleted.Callback += OnPuzzleComplete;
    //}

    //private void OnPuzzleComplete(SyncList<PuzzleId>.Operation op, int itemIndex, PuzzleId oldItem, PuzzleId newItem)
    //{
    //    if(op == SyncList<PuzzleId>.Operation.OP_ADD)
    //    {

    //    }
    //}

    public int PuzzlesDone { get => puzzlesCompleted.Count; }

    /// <summary>
    /// Register puzzle as completed
    /// </summary>
    /// <param name="id"></param>
    [Server]
    public void MarkCompleted(PuzzleId id)
    {
        OnPuzzleCompleted?.Invoke(id);
        puzzlesCompleted.Add(id);
    }
}
