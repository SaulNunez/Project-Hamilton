using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletransport : InteractuableBehavior
{
    public enum GoUpOn
    {
        OriginVent,
        TeletransportVent
    }

    [Tooltip("Destination of vent, only reads transform's position")]
    public Transform teletransportTo;

    public override void OnApproach(GameObject approachedBy)
    {
        base.OnApproach(approachedBy);

        // Only impostors can teletransport
        var killingComponent = approachedBy.GetComponent<Killing>();
        if(killingComponent != null && killingComponent.canKill == true)
        {
            return;
        }

        var netIdentityOfApproached = approachedBy.GetComponent<NetworkIdentity>();
        if (netIdentityOfApproached != null)
        {
            print($"Teletransport manager available: {TeletransportManager.instance != null}");
            TeletransportManager.instance.AddToTeletransportList(netIdentityOfApproached, this.gameObject);
        }
        else
        {
            Debug.LogError($"Gameobject {approachedBy.name} doesn't containe a network identity");
        }
    }

}
