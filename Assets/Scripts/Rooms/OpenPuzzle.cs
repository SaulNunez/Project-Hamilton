using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;
using System.Linq;

public class OpenPuzzle : InteractuableBehavior
{
    [SyncVar]
    public bool puzzleIsAvailable = true;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    PuzzleId opens;

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
