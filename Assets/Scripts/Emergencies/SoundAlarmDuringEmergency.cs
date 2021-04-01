using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For auditory alarm. 
/// Starts alarm on emergency start. Ends when resolved or players lost.
/// 
/// Events are listened on server and an RPC call controls the AudioSource on the client.
/// </summary>
public class SoundAlarmDuringEmergency : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("An AudioSource to control to play during emergencies")]
    AudioSource audioSource;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Emergency.OnEmergencyStarted += RpcOnEmergencyStart;
        Emergency.OnEmergencyResolved += RpcOnEmergencyEnded;
        Emergency.OnPlayersCouldntStopEmergency += RpcOnEmergencyEnded;
    }

    [ClientRpc]
    private void RpcOnEmergencyEnded()
    {
        audioSource.Stop();
    }

    [ClientRpc]
    private void RpcOnEmergencyStart(Emergency.EmergencyType _)
    {
        audioSource.Play();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        Emergency.OnEmergencyStarted -= RpcOnEmergencyStart;
        Emergency.OnEmergencyResolved -= RpcOnEmergencyEnded;
        Emergency.OnPlayersCouldntStopEmergency -= RpcOnEmergencyEnded;
    }
}
