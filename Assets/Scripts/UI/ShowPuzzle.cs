using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles opening the puzzle UI with a simple interface for other code. Also handles closing UI when there's voting.
/// </summary>
public class ShowPuzzle : NetworkBehaviour
{
    [SerializeField]
    GameObject boilerInt;
    [SerializeField]
    GameObject sequence;
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


    public static ShowPuzzle instance = null;

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += StopCurrentPuzzle;

        if(instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogError($"Another instance of component was found.");
        }
    }

    private void StopCurrentPuzzle(int _)
    {
        boilerInt.SetActive(false);
        sequence.SetActive(false);
        variablesString.SetActive(false);
        variablesBool.SetActive(false);
        doWhileCarStarter.SetActive(false);
        floatThermostat.SetActive(false);
        forWashing.SetActive(false);
        whileFillWaterBucket.SetActive(false);
        ifPickFlower.SetActive(false);
        substring.SetActive(false);
        ifelseCake.SetActive(false);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= StopCurrentPuzzle;
    }

    /// <summary>
    /// Opens puzzle
    /// </summary>
    /// <param name="puzzle">Id of puzzle</param>
    public void OpenPuzzles(PuzzleId puzzle)
    {

        if (isServer)
        {
            RpcOpenPuzzleOnClient(puzzle);
        }

        if(isClient)
        {
            ActivatePuzzleOnClient(puzzle);
        }
        
    }

    [ClientRpc]
    private void RpcOpenPuzzleOnClient(PuzzleId puzzle)
    {
        ActivatePuzzleOnClient(puzzle);
    }

    [Client]
    private void ActivatePuzzleOnClient(PuzzleId puzzle)
    {
        if (hasAuthority)
        {
            switch (puzzle)
            {
                case PuzzleId.BoilersVariableInteger:
                    boilerInt.SetActive(true);
                    break;
                case PuzzleId.Sequence1:
                    sequence.SetActive(true);
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
        }
    }
}
