using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be used on the emergency button.
/// Sends a RPC call to turn off collisions when emergency has started 
/// and turns them back on when the emergency has been solved.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DisableWhenOnEmergency : NetworkBehaviour
{
    Collider2D colliderInGameObject;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Emergency.OnEmergencyResolved += RpcOnEmergencyResolved;
        Emergency.OnEmergencyStarted += RpcOnEmergencyStarted;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        Emergency.OnEmergencyResolved -= RpcOnEmergencyResolved;
        Emergency.OnEmergencyStarted -= RpcOnEmergencyStarted;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        colliderInGameObject = GetComponent<Collider2D>();
    }

    [ClientRpc]
    private void RpcOnEmergencyResolved()
    {
        colliderInGameObject.enabled = true;
    }

    [ClientRpc]
    private void RpcOnEmergencyStarted()
    {
        colliderInGameObject.enabled = false;
    }

}
