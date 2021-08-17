using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDead : NetworkBehaviour
{
    private bool detected;

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

    public override void OnStartClient()
    {
        base.OnStartClient();

        var hubConfigGO = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        config = hubConfigGO.GetComponent<HubConfig>();

        GameUI.OnReportClick += CmdReport;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        GameUI.OnReportClick -= CmdReport;
    }

    void Update()
    {
        if (isClient)
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
    private void CmdReport()
    {
        var found = Physics2D.OverlapCircle(transform.position, config.actDistance, deadPlayersLayers);
        if (found)
        {
            var votingManagerGameObject = GameObject.FindGameObjectWithTag(Tags.VotingManager);
            var votingManager = votingManagerGameObject.GetComponent<VotingManager>();

            votingManager.StartVoting();
        }
    }
}
