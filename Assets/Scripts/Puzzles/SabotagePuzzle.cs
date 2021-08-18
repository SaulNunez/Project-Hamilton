using Extensions;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base for sabotages. Has facilities for puzzles that require multiple players to respond it in a time limit to be marked as done.
/// 
/// Opening puzzle is done by the client. Closing will be done automagically from the server, either when sabotage is solved (closing in all clients) or when a player gets it right.
/// </summary>
public class SabotagePuzzle : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField]
    [Tooltip("UI that players interact to solve puzzle")]
    protected GameObject ui;

    /// <summary>
    /// Is UI for puzzle currently active
    /// </summary>
    [SyncVar]
    private bool isPuzzleEnabled = false;

    [Header("Multiple users")]
    [Range(1, 6)]
    [SerializeField]
    int requireNumberOfPlayersToSolve = 2;

    [Range(1f, 90f)]
    [SerializeField]
    float timeoutSecondsBeforeClearing = 20f;

    /// <summary>
    /// Enables opening a UI for solving puzzle primaraly
    /// </summary>
    protected bool IsPuzzleEnabled
    {
        get => isPuzzleEnabled;
        set
        {
            if (value == true)
            {
                OnPuzzleActivated();
            }

            isPuzzleEnabled = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    private void TurnEmergencyIfNecessary(Emergency.EmergencyType type)
    {
        if (AreEmergencyConditionsEnough(type))
        {
            IsPuzzleEnabled = true;
        }
    }

    /// <summary>
    /// Called after an emergency ocurred. 
    /// The returned bool is if all conditions to trigger emergency are ready. 
    /// Regularly just checking for the emergency type is enough.
    /// 
    /// </summary>
    protected virtual bool AreEmergencyConditionsEnough(Emergency.EmergencyType type)
    {
        return false;
    }

    private void TurnEmergencyOff()
    {
        IsPuzzleEnabled = false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += ResetSabotage;
        Emergency.OnEmergencyStarted += TurnEmergencyIfNecessary;
        Emergency.OnEmergencyResolved += TurnEmergencyOff;
    }

    protected virtual void ResetSabotage(int _)
    {
         
    }

    /// <summary>
    /// Shows sabotage UI.
    /// 
    /// Note. Might not activate if for example, this sabotage isn't active.
    /// </summary>
    [Client]
    public void ShowPuzzle()
    {
        if (IsPuzzleEnabled)
        {
            ui.SetActive(true);
        }
    }

    /// <summary>
    /// Called when puzzle was shown.
    /// </summary>
    /// <remarks>
    /// Should be called on server
    /// </remarks>
    [Server]
    protected virtual void OnPuzzleActivated() { }

    [Server]
    protected virtual bool ArePuzzleCompletionConditionsReached() => false;

    /// <summary>
    /// Call on the server when the puzzle has been finished by the user.
    /// </summary>
    [Server]
    protected void SetPuzzleAsCompleted(NetworkConnection player)
    {

        TargetClosePuzzle(player);

        if (ArePuzzleCompletionConditionsReached())
        {
            this.SuperPrint("Sabotage completed!");
            OnPuzzleCompleted();
        }
    }

    /// <summary>
    /// Implement on inheritors to set the action to do when the puzzle has been solved by the players
    /// 
    /// Allways call base implementation via `base.OnPuzzleCompleted()` or some default behaviour will be missing!
    /// </summary>
    /// <remarks>
    /// Dont' call directly
    /// </remarks>
    [Server]
    protected virtual void OnPuzzleCompleted() {
        TurnEmergencyOff();
        CloseAllPuzzle();
        ResetSabotage(0);

        Emergency.instance.StopEmergency();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= ResetSabotage;
        Emergency.OnEmergencyStarted -= TurnEmergencyIfNecessary;
        Emergency.OnEmergencyResolved -= TurnEmergencyOff;
    }

    [TargetRpc]
    protected void TargetClosePuzzle(NetworkConnection target)
    {
        ui.SetActive(false);
    }

    [ClientRpc]
    protected void CloseAllPuzzle()
    {
        ui.SetActive(false);
    }
}
