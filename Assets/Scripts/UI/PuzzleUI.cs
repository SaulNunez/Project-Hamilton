using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles logic of puzzle UI with a simple interface for other code. 
/// * Handles closing UI when there's voting.
/// * Handles opening puzzle.
/// * Handles closing puzzle when user presses `ESC`
/// </summary>
public class PuzzleUI : NetworkBehaviour
{
    [SerializeField]
    GameObject boilerInt;
    [SerializeField]
    GameObject sequence;
    [SerializeField]
    GameObject sequence2;
    [SerializeField]
    GameObject sequence3;
    [SerializeField]
    GameObject variablesString;
    [SerializeField]
    GameObject variablesBool;
    [SerializeField]
    GameObject doWhileCarStarter;
    [SerializeField]
    GameObject floatThermostat;
    [SerializeField]
    GameObject forWashing;
    [SerializeField]
    GameObject whileFillWaterBucket;
    [SerializeField]
    GameObject ifPickFlower;
    [SerializeField]
    GameObject substring;
    [SerializeField]
    GameObject ifelseCake;

    /// <summary>
    /// Called when a puzzle UI is to be shown on a specific client.
    /// </summary>
    /// <remarks>
    /// Only applicable on clients
    /// </remarks>
    public static event Action OnOpenPuzzleInClient;

    /// <summary>
    /// Called when close puzzle is called, there might be a screen visible but it's not guaranteed.
    /// </summary>
    /// <remarks>
    /// Only applicable on clients
    /// </remarks>
    public static event Action OnClosePuzzleInClient;


    public static PuzzleUI instance = null;

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += RpcStopCurrentPuzzle;

        if (instance == null)
        {
            instance = this;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzles();
        }

    }

    /// <summary>
    /// Close all open  puzzles in all clients
    /// </summary>
    /// <param name="_"></param>
    [ClientRpc]
    private void RpcStopCurrentPuzzle(int _)
    {
        ClosePuzzles();
    }

    /// <summary>
    /// Usable in the client, closes any puzzle on screen
    /// </summary>
    [Client]
    public void ClosePuzzles()
    {
        boilerInt.SetActive(false);
        sequence.SetActive(false);
        sequence2.SetActive(false);
        variablesString.SetActive(false);
        variablesBool.SetActive(false);
        doWhileCarStarter.SetActive(false);
        floatThermostat.SetActive(false);
        forWashing.SetActive(false);
        whileFillWaterBucket.SetActive(false);
        ifPickFlower.SetActive(false);
        substring.SetActive(false);
        ifelseCake.SetActive(false);

        OnClosePuzzleInClient?.Invoke();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= RpcStopCurrentPuzzle;
    }

    /// <summary>
    /// Opens puzzle
    /// </summary>
    /// <param name="puzzle">Id of puzzle</param>
    /// <param name="openedBy">GameObject that interacted, a player (that contains a NetworkIdentity)</param>
    [Server]
    public void OpenPuzzles(PuzzleId puzzle, GameObject openedBy)
    {
        var openedByNetworkIdentity = openedBy.GetComponent<NetworkIdentity>();
        if (openedByNetworkIdentity == null)
        {
            return;
        }

        TargetOpenPuzzleOnClient(openedByNetworkIdentity.connectionToClient, puzzle);
    }

    [TargetRpc]
    private void TargetOpenPuzzleOnClient(NetworkConnection target, PuzzleId puzzle)
    {
        ActivatePuzzleOnClient(puzzle);
    }

    [Client]
    private void ActivatePuzzleOnClient(PuzzleId puzzle)
    {
        switch (puzzle)
        {
            case PuzzleId.BoilersVariableInteger:
                boilerInt.SetActive(true);
                break;
            case PuzzleId.Sequence1:
                sequence.SetActive(true);
                break;
            case PuzzleId.Sequence2:
                sequence2.SetActive(true);
                break;
            case PuzzleId.VariableString:
                variablesString.SetActive(true);
                break;
            case PuzzleId.VariableBoolean:
                variablesBool.SetActive(true);
                break;
            case PuzzleId.DoWhileMotorStarter:
                doWhileCarStarter.SetActive(true);
                break;
            case PuzzleId.VariableFloat:
                floatThermostat.SetActive(true);
                break;
            case PuzzleId.ForWashingBucket:
                forWashing.SetActive(true);
                break;
            case PuzzleId.WhileFillingBucket:
                whileFillWaterBucket.SetActive(true);
                break;
            case PuzzleId.IfFlowerPicking:
                ifPickFlower.SetActive(true);
                break;
            case PuzzleId.Substring:
                substring.SetActive(true);
                break;
            case PuzzleId.IfElse:
                ifelseCake.SetActive(true);
                break;
        }

        OnOpenPuzzleInClient?.Invoke();
    }
}
