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
    public enum EmergencyType
    {
        None,
        TurnDownGenerator,
        ChangeBoilerPressure,
        QuestionSabotage
    }


    /// <summary>
    /// Hub config
    /// </summary>
    /// <remarks>
    /// Only available on server
    /// </remarks>
    private HubConfig hubConfig;

    [SyncVar(hook = nameof(OnSabotageAvailabilityChanged))]
    private bool areEmergenciesAvailable = false;

    [SyncVar]
    private int timeRemainingOnWaiting;

    [SyncVar]
    private bool onSabotage = false;

    public EmergencyType CurrentActiveSabotage { get => currentActiveSabotage;  }

    [SyncVar]
    private EmergencyType currentActiveSabotage = EmergencyType.None;

    [SyncVar]
    private int timeRemainingOnEmergency;

    [Header("UI Elements")]
    [Tooltip("Button for activating boiler sabotage")]
    [SerializeField]
    Button boilerSabotage;

    [Tooltip("Button for activating electricity sabotage")]
    [SerializeField]
    Button electricitySabotage;

    [Tooltip("Button for activating telegram sabotage")]
    [SerializeField]
    Button questionsSabotage;

    /// <summary>
    /// Single instance of the class.
    /// </summary>
    /// <remarks>
    /// Only available on server and client
    /// </remarks>
    public static Emergency instance = null;

    /// <summary>
    /// Called when an emergency is finished, when solved by several persons.
    /// </summary>
/// <remarks>
    /// Only available on server.
    /// </remarks>
    public static event Action<EmergencyType> OnEmergencyStarted;

    /// <summary>
    /// Called when time remaining changed. This is the time remaining to solve an emergency, useful for updating UI.
    /// </summary>
    /// <remarks>
    /// Only available on server.
    /// </remarks>
    public static event Action<int> OnTimeRemaingForEmergencyChanged;

    /// <summary>
    /// Called when an emergency is finished, when solved by several persons.
    /// </summary>
    /// <remarks>
    /// Only available on server.
    /// </remarks>
    public static event Action OnEmergencyResolved;


    /// <summary>
    /// Called when the emergency couldn't be stoped, this means players loose.
    /// </summary>
    /// <remarks>
    /// Only available on server
    /// </remarks>
    public static event Action OnPlayersCouldntStopEmergency;

    private void OnSabotageAvailabilityChanged(bool oldValue, bool newValue)
    {
        boilerSabotage.interactable = newValue;
        electricitySabotage.interactable = newValue;
        questionsSabotage.interactable = newValue;
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

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (instance != null)
        {
            instance = this;
        }
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
        onSabotage = false;

        OnEmergencyResolved?.Invoke();
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

    /// <summary>
    /// For UI on client, starts emergency sabotaging electric generators 
    /// </summary>
    [Client]
    public void StartElectricitySabotage() => CmdStartEmergency(EmergencyType.TurnDownGenerator);

    /// <summary>
    /// For UI on client, starts emergency sabotaging boiler pressure 
    /// </summary>
    [Client]
    public void StartBoilerSabotage() => CmdStartEmergency(EmergencyType.ChangeBoilerPressure);

    [Client]
    public void StartTelegraphSabotage() => CmdStartEmergency(EmergencyType.QuestionSabotage);

    [Command(ignoreAuthority = true)]
    public void CmdStartEmergency(EmergencyType sabotageType)
    {
        if (!areEmergenciesAvailable)
        {
            return;
        }

        areEmergenciesAvailable = false;
        onSabotage = true;

        OnEmergencyStarted?.Invoke(sabotageType);

        currentActiveSabotage = sabotageType;
        Invoke(nameof(OnTimeEnded), hubConfig.secondsEmergencyDuration);
        StartCountdownForEmergency();
    }

    private void CountdownTimeRemaining()
    {
        timeRemainingOnEmergency--;
        if(timeRemainingOnEmergency > 0)
        {
            OnTimeRemaingForEmergencyChanged?.Invoke(timeRemainingOnEmergency);
            Invoke(nameof(CountdownTimeRemaining), 1f);
            return;
        }
        OnTimeEnded();
    }

    [Server]
    private void OnTimeEnded()
    {
        OnPlayersCouldntStopEmergency?.Invoke();
        //TODO: Connect with end game system
    }

    [Server]
    public void StopEmergency()
    {
        CancelInvoke(nameof(OnTimeEnded));
        CancelInvoke(nameof(CountdownToAvailability));
        onSabotage = false;
        currentActiveSabotage = EmergencyType.None;
        StartCooldown();

        OnEmergencyResolved?.Invoke();
    }

    void StartCountdownForEmergency()
    {
        timeRemainingOnEmergency = hubConfig.secondsEmergencyDuration;
        Invoke(nameof(CountdownTimeRemaining), 1f);
    }
}
