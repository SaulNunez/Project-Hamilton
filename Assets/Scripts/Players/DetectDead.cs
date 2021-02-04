using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDead : NetworkBehaviour
{
    [SyncVar(hook = nameof(DetectedDeadPlayerChanged))]
    private bool detected;

    private void DetectedDeadPlayerChanged(bool oldValue, bool newValue)
    {
        
    }

    [SerializeField]
    private LayerMask deadPlayersLayers;

    HubConfig config;

    Collider2D other;

    public override void OnStartServer()
    {
        base.OnStartServer();

        var hubConfigGO = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        config = hubConfigGO.GetComponent<HubConfig>();
    }

    void Update()
    {
        if (isServer)
        {
            var found = Physics2D.OverlapCircle(transform.position, config.actDistance, deadPlayersLayers);
            if (found)
            {
                other = found;
                detected = true;
            } else
            {
                detected = false;
            }
        }
    }

    [Command]
    public void CmdReport()
    {
        if (detected)
        {
            var votingManagerGameObject = GameObject.FindGameObjectWithTag(Tags.VotingManager);
            var votingManager = votingManagerGameObject.GetComponent<VotingManager>();

            votingManager.StartVoting();
        }
    }
}
