using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acknowledges when player has interacted with teletransport vent, and adds player to teletransport manager to handle on teletransport options and such
/// </summary>
public class Teletransport : NetworkBehaviour, IInteractuableBehaviour
{
    public enum GoUpOn
    {
        OriginVent,
        TeletransportVent
    }

    [Tooltip("Destination of vent, only reads transform's position")]
    public Transform teletransportTo;

    public void OnApproach(GameObject approachedBy)
    {
        // Only impostors can teletransport
        var killingComponent = approachedBy.GetComponent<Killing>();
        var netIdentityOfApproached = approachedBy.GetComponent<NetworkIdentity>();
        if (netIdentityOfApproached != null && killingComponent != null && killingComponent.IsAssasin)
        {
            TeletransportManager.instance.AddToTeletransportList(netIdentityOfApproached, this.gameObject);
        }
        else
        {
            Debug.LogError($"Gameobject {approachedBy.name} doesn't containe a network identity or can't teletransport");
        }
    }

}
