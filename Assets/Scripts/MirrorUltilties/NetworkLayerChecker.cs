using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLayerChecker : NetworkVisibility
{
    public LayerMask visibleGameObjects;

    /// <summary>
    /// How often (in seconds) that this object should update the list of observers that can see it.
    /// </summary>
    [Tooltip("How often (in seconds) that this object should update the list of observers that can see it.")]
    public float visUpdateInterval = 1;

    public override void OnStartServer()
    {
        InvokeRepeating(nameof(RebuildObservers), 0, visUpdateInterval);
    }
    public override void OnStopServer()
    {
        CancelInvoke(nameof(RebuildObservers));
    }

    void RebuildObservers()
    {
        netIdentity.RebuildObservers(false);
    }

    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return (visibleGameObjects.value & (1 << conn.identity.gameObject.layer)) != 0;
    }

    /// <summary>
    /// Callback used by the visibility system to (re)construct the set of observers that can see this object.
    /// <para>Implementations of this callback should add network connections of players that can see this object to the observers set.</para>
    /// </summary>
    /// <param name="observers">The new set of observers for this object.</param>
    /// <param name="initialize">True if the set of observers is being built for the first time.</param>
    public override void OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        // 'transform.' calls GetComponent, only do it once
        Vector3 position = transform.position;

        // brute force distance check
        // -> only player connections can be observers, so it's enough if we
        //    go through all connections instead of all spawned identities.
        // -> compared to UNET's sphere cast checking, this one is orders of
        //    magnitude faster. if we have 10k monsters and run a sphere
        //    cast 10k times, we will see a noticeable lag even with physics
        //    layers. but checking to every connection is fast.
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn != null && conn.identity != null)
            {
                // Brute force layer check
                if ((visibleGameObjects.value & (1 << conn.identity.gameObject.layer)) != 0)
                {
                    observers.Add(conn);
                }
            }
        }
    }
}
