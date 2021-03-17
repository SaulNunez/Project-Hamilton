using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base for sabotages. Has facilities for puzzles that require multiple players to respond it in a time limit to be marked as done.
/// </summary>
public class SabotagePuzzle : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField]
    [Tooltip("UI that players interact to solve puzzle")]
    protected GameObject ui;

    [SyncVar]
    bool isPuzzleEnabled = false;

    [Header("Multiple users")]
    [Range(1, 6)]
    [SerializeField]
    int requireNumberOfPlayersToSolve = 2;

    [Range(1f, 90f)]
    [SerializeField]
    float timeoutSecondsBeforeClearing = 20f;

    void OnPuzzleEnabledChanged(bool oldValue, bool newValue)
    {
        ui.SetActive(newValue);
    }

    /// <summary>
    /// Connections of those who have solved the puzzle
    /// </summary>
    /// <remarks>
    /// Only available on the server
    /// </remarks>
    private List<NetworkConnection> playersWhoSolved = new List<NetworkConnection>();

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += ClearOnVoting;
    }

    private void ClearOnVoting(int obj)
    {
        CancelInvoke(nameof(ClearSolvedOnTimeout));
        ClearSolvedOnTimeout();
    }

    void ClearSolvedOnTimeout()
    {
        playersWhoSolved.Clear();
    }

    /// <summary>
    /// Call on the server when the puzzle has been finished by the user.
    /// </summary>
    [Server]
    protected void SetPuzzleAsCompleted(NetworkConnection player)
    {
        //Reset timeout, check enough users have completed
        CancelInvoke(nameof(ClearSolvedOnTimeout));

        playersWhoSolved.Add(player);

        if (playersWhoSolved.Count == requireNumberOfPlayersToSolve)
        {
            OnPuzzleCompleted();
        }

        Invoke(nameof(ClearSolvedOnTimeout), timeoutSecondsBeforeClearing);
    }

    /// <summary>
    /// Implement on inheritors to set the action to do when the puzzle has been solved by the players
    /// </summary>
    protected virtual void OnPuzzleCompleted() { }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= ClearOnVoting;
    }

    protected void ClosePuzzle()
    {
        isPuzzleEnabled = true;
    }
}
