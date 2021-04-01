using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSabotage : InteractuableBehavior
{
    [SyncVar(hook = nameof(UpdateVisuals))]
    bool sabotageIsAvailable = false;

    [SerializeField]
    SabotagePuzzle sabotageToSolve;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Emergency.OnEmergencyStarted += EnableButtonOnEmergency;
        Emergency.OnEmergencyResolved += TemporaryDisableButtonOnEmergencyStop;
    }

    void UpdateVisuals(bool oldValue, bool newValue)
    {
        var materialSet = GetComponent<PuzzleActiveOutline>();
        materialSet.IsActive = newValue;
    }

    private void TemporaryDisableButtonOnEmergencyStop()
    {
        sabotageIsAvailable = false;
    }

    private void EnableButtonOnEmergency(Emergency.EmergencyType _)
    {
        sabotageIsAvailable = true;
    }

    public override void OnApproach(GameObject approachedBy)
    {
        base.OnApproach(approachedBy);

        if (sabotageIsAvailable)
        {
            var networkIdentity = approachedBy.GetComponent<NetworkIdentity>();
            if(netIdentity != null)
            {
                TargetOpenSabotageUI(networkIdentity.connectionToClient);
            }
        }
    }

    [TargetRpc]
    void TargetOpenSabotageUI(NetworkConnection target)
    {
        sabotageToSolve.ShowPuzzle();
    }


    public override void OnStopServer()
    {
        base.OnStopServer();
    }
}
