using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;
using System.Linq;

public class OpenPuzzle : NetworkBehaviour, IInteractuableBehaviour
{
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

    private void OnPuzzleCompleted(PuzzleId obj, NetworkIdentity doneByPlayer)
    {
        if(obj == opens)
        {
            TargetUpdatePuzzleStatus(doneByPlayer.connectionToClient, false);
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= OnPuzzleCompleted;
    }

    [TargetRpc]
    void TargetUpdatePuzzleStatus(NetworkConnection target, bool enabled)
    {
        puzzleIsAvailable = enabled;

        var materialSet = GetComponent<PuzzleActiveOutline>();
        materialSet.IsActive = enabled;
    }

    [Server]
    public void OnApproach(GameObject approachedBy)
    {
        if (puzzleIsAvailable)
        {
            print("Show puzzle");
            //TODO: Crear id de puzzle en el servidor para mantener el estado
            PuzzleUI.instance.OpenPuzzles(opens, approachedBy);
        }
    }
}
