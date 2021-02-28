using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;
using System.Linq;

public class OpenPuzzle : InteractuableBehavior
{
    [SyncVar(hook = nameof(UpdateVisuals))]
    public bool puzzleIsAvailable = true;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    PuzzleId opens;

    public override void OnStartServer()
    {
        base.OnStartServer();

        PuzzleCompletion.OnPuzzleCompleted += OnPuzzleCompleted;
    }

    private void OnPuzzleCompleted(PuzzleId obj)
    {
        if(obj == opens)
        {
            puzzleIsAvailable = false;
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= OnPuzzleCompleted;
    }

    void UpdateVisuals(bool oldValue, bool newValue)
    {
        var materialSet = GetComponent<PuzzleActiveOutline>();
        materialSet.IsActive = newValue;
    }

    [Server]
    public override void OnApproach(GameObject approachedBy)
    {
        if (puzzleIsAvailable)
        {
            print("Show puzzle");
            //TODO: Crear id de puzzle en el servidor para mantener el estado
            //TargetOpenPuzzleOnPlayer(approachedBy.GetComponent<NetworkTransform>().connectionToClient);
            var puzzleManager = approachedBy.GetComponent<ShowPuzzle>();
            puzzleManager.OpenPuzzles(opens);
        }
    }
}
