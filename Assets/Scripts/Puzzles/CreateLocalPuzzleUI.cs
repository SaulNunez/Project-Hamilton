﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLocalPuzzleUI : NetworkBehaviour
{
    [SerializeField]
    private GameObject puzzleUiPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();

        CmdSpawnPuzzleIdOnServer();
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnPuzzleIdOnServer(NetworkConnectionToClient sender = null)
    {
        var uiCanvas = GameObject.FindGameObjectWithTag(Tags.UiManager);

        var ui = Instantiate(puzzleUiPrefab, uiCanvas.transform);

        NetworkServer.Spawn(ui, sender);
    }
}
