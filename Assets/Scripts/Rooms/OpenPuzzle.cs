using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;
using System.Linq;

public class OpenPuzzle : InteractuableBehavior
{
    HubConfig hubConfig;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    PuzzleId opens;

    public override void OnStartServer()
    {
        var lobbyConfigs = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = lobbyConfigs.GetComponent<HubConfig>();
    }

    [Server]
    public override void OnApproach(GameObject approachedBy)
    {
        //TODO: Crear id de puzzle en el servidor para mantener el estado
        TargetOpenPuzzleOnPlayer(approachedBy.GetComponent<NetworkTransform>().connectionToClient);
    }

    [TargetRpc]
    void TargetOpenPuzzleOnPlayer(NetworkConnection target)
    {
        ShowPuzzle.Instance.OpenPuzzles(opens);
    }
}
