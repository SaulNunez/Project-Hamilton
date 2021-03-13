using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emergency : NetworkBehaviour
{
    /// <summary>
    /// Hub config
    /// </summary>
    /// <remarks>
    /// Only available on server
    /// </remarks>
    private HubConfig hubConfig;

    [SyncVar]
    public bool isElectricSabotageInProgress;

    [SyncVar]
    public bool isBoilerSabotageInProgress;

    public bool IsSabotageActive { get => isElectricSabotageInProgress || isBoilerSabotageInProgress; }

    private int timeRemaing;

    public static Emergency instance = null;

    /// <summary>
    /// Called when time remaining changed. This is the time remaining to solve an emergency, useful for updating UI.
    /// </summary>
    /// <remarks>
    /// Only available on client.
    /// </remarks>
    public static event Action<int> OnTimeRemaingForEmergencyChanged;

    /// <summary>
    /// Called when an emergency is finished, when solved by several persons.
    /// </summary>
    public static event Action OnEmergencyResolved;


    public override void OnStartServer()
    {
        base.OnStartServer();

        var hubGo = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = hubGo.GetComponent<HubConfig>();

        if(instance != null)
        {
            instance = this;
        }
    }

    [Server]
    public void StartEmergency(PuzzleId puzzleId)
    {
        if (IsSabotageActive)
        {
            return;
        }

        switch (puzzleId)
        {
            case PuzzleId.SabotageBoilerPressure:
                isBoilerSabotageInProgress = true;
                Invoke(nameof(OnTimeEnded), hubConfig.secondsEmergencyDuration);
                RpcStartCountdownForEmergency(hubConfig.secondsEmergencyDuration);
                break;
            case PuzzleId.SabotageElectricity:
                isElectricSabotageInProgress = true;
                RpcStartCountdownForEmergency(hubConfig.secondsEmergencyDuration);
                Invoke(nameof(OnTimeEnded), hubConfig.secondsEmergencyDuration);
                break;
        }
    }

    [Client]
    private void Countdown()
    {
        timeRemaing--;
        if(timeRemaing > 0)
        {
            Invoke(nameof(Countdown), 1f);
        }
    }

    [Server]
    private void OnTimeEnded()
    {
        isBoilerSabotageInProgress = false;
        isElectricSabotageInProgress = false;
        //TODO: Connect with end game system
    }

    [Server]
    public void StopEmergency()
    {
        CancelInvoke(nameof(OnTimeEnded));
        isBoilerSabotageInProgress = false;
        isElectricSabotageInProgress = false;

        OnEmergencyResolved?.Invoke();
    }

    [ClientRpc]
    public void RpcStartCountdownForEmergency(int time)
    {
        timeRemaing = time;
        Invoke(nameof(Countdown), 1f);
    }
}
