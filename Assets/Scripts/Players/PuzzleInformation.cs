using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PuzzleInformation : NetworkBehaviour
{
    public SyncList<string> usedPuzzles = new SyncList<string>();

    public SyncList<string> failedPuzzles = new SyncList<string>();

    [Server]
    public void SetPuzzleResult(string puzzleId, bool passed)
    {
        usedPuzzles.Add(puzzleId);
        if (!passed)
        {
            failedPuzzles.Add(puzzleId);
        }
    }
}
