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

    public override void OnStartServer()
    {
        var lobbyConfigs = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = lobbyConfigs.GetComponent<HubConfig>();
    }

    [Server]
    public override void OnApproach(GameObject approachedBy)
    {   
        print("aaa");
        //Checar en el servidor que el jugador este cerca
        if(Vector2.Distance(approachedBy.transform.position,transform.position) <= hubConfig.actDistance){
            
        }
    }
}
