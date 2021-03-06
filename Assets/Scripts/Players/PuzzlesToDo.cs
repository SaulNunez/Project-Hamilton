﻿using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Currently unused. Expected to be used when implementing a list of tasks by user
/// </summary>
public class PuzzlesToDo : NetworkBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public SyncDictionary<PuzzleId, PuzzleInformation> puzzles = new SyncDictionary<PuzzleId, PuzzleInformation>();

    public override void OnStartServer()
    {
        base.OnStartServer();

        var hubConfig = GameObject.FindGameObjectWithTag(Tags.HubConfig).GetComponent<HubConfig>();

        var maxPuzzle = Enum.GetValues(typeof(PuzzleId)).Cast<PuzzleId>().Max();

        while(puzzles.Count < hubConfig.numberOfTasks)
        {
            var puzzleToDoInt = Random.Range(0, (int)maxPuzzle);

            if (!puzzles.ContainsKey((PuzzleId)puzzleToDoInt))
            {
                puzzles.Add((PuzzleId)puzzleToDoInt, new PuzzleInformation());
            }
        }

        TargetDisableNonActionablePuzzles();
    }

    [TargetRpc]
    private void TargetDisableNonActionablePuzzles()
    {

    }
}
