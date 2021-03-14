using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles emergency logic, like activating buttons to turn emergency types. 
/// Also handles wait to activate, and progress; offers countdown event for clients.
///    
/// Emergencies have three states:
/// 1. Waiting to activate
///     * After voting, there's a wait before them being available
///     * When a player uses them there's another wait before being available
/// 2. Available (assasins can activate them)
/// 3. In progress, when an assasin used them, players are currently scrambling to solve it. Failure to do so finishes the game with victory to the assasins.
/// 
/// </summary>
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
    private bool areEmergenciesAvailable = false;

    [SyncVar]
    private int timeRemainingOnWaiting;

    [SyncVar]
    private bool onSabotage = false;

    [SyncVar]
    private PuzzleId currentActiveSabotage;

    [SyncVar]
    private int timeRemainingOnEmergency;

    [Header("UI Elements")]
    [Tooltip("Button for activating boiler sabotage")]
    [SerializeField]
    Button boilerSabotage;

    [Tooltip("Button for activating electricity sabotage")]
    [SerializeField]
    Button electricitySabotage;

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

    private void OnBoilerSabotageAvailabilityChange(bool oldValue, bool newValue)
    {
        boilerSabotage.interactable = newValue;
    }

    private void OnElectricitySabotageAvailabilityChange(bool oldValue, bool newValue)
    {
        electricitySabotage.interactable = newValue;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var hubGo = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = hubGo.GetComponent<HubConfig>();

        if(instance != null)
        {
            instance = this;
        }

        VotingManager.OnVotingEnded += StartCooldown;
        VotingManager.OnVotingStarted += StopSabotageOnVotingStarted;

        StartCooldown();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingEnded -= StartCooldown;
        VotingManager.OnVotingStarted -= StopSabotageOnVotingStarted;
    }

    private void StopSabotageOnVotingStarted(int _)
    {
        areEmergenciesAvailable = false;
    }

    /// <summary>
    /// Set time remaining. Called for starting the cooldown timer.
    /// </summary>
    private void StartCooldown()
    {
        timeRemainingOnWaiting = hubConfig.secondsOfCooldownForSabotage;
        Invoke(nameof(CountdownToAvailability), 1);
    }

    private void CountdownToAvailability()
    {
        timeRemainingOnWaiting--;
        if(timeRemainingOnWaiting > 0) {
            Invoke(nameof(CountdownToAvailability), 1);
            return;
        }

        areEmergenciesAvailable = true;
    }

    [Server]
    public void StartEmergency(PuzzleId puzzleId)
    {
        if (!areEmergenciesAvailable)
        {
            return;
        }

        areEmergenciesAvailable = false;
        onSabotage = true;

        currentActiveSabotage = puzzleId;
        Invoke(nameof(OnTimeEnded), hubConfig.secondsEmergencyDuration);
        StartCountdownForEmergency();
    }

    private void CountdownTimeRemaining()
    {
        timeRemainingOnEmergency--;
        if(timeRemainingOnEmergency > 0)
        {
            Invoke(nameof(CountdownTimeRemaining), 1f);
            return;
        }
        OnTimeEnded();
    }

    [Server]
    private void OnTimeEnded()
    {
        //TODO: Connect with end game system
    }

    [Server]
    public void StopEmergency()
    {
        CancelInvoke(nameof(OnTimeEnded));
        onSabotage = false;
        StartCooldown();

        OnEmergencyResolved?.Invoke();
    }

    public void StartCountdownForEmergency()
    {
        timeRemainingOnEmergency = hubConfig.secondsEmergencyDuration;
        Invoke(nameof(CountdownTimeRemaining), 1f);
    }
}
