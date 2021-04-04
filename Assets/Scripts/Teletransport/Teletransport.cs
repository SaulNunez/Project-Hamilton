using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletransport : InteractuableBehavior
{
    public Transform teletransportTo;

    public override void OnApproach(GameObject approachedBy)
    {
        base.OnApproach(approachedBy);
        
        //Teletransport is done server side, and server doesn't have authority in gameobject
        var networkTransform = approachedBy.GetComponent<NetworkTransform>();
        networkTransform.ServerTeleport(teletransportTo.position);
    }
}
