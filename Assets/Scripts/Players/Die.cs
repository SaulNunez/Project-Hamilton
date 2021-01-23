using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : InteractuableBehavior
{
    public bool isDead = false;

    readonly LayerMask layerMask = LayerMask.NameToLayer(Layers.Players);

    HubConfig hubConfig;


    public override void OnStartServer()
    {
        var lobbyConfigs = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = lobbyConfigs.GetComponent<HubConfig>();
    }

    [Server]
    public override void OnApproach(GameObject approachedBy)
    {
        base.OnApproach(approachedBy);

        Collider2D somethingNear = Physics2D.OverlapCircle(transform.position, hubConfig.actDistance, layerMask);
        if (somethingNear)
        {
        }
    }
}
