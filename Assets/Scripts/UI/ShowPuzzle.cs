using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPuzzle : NetworkBehaviour
{
    [Header("Puzzles")]
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
    [SerializeField]
    GameObject sabotagePresure;
    [SerializeField]
    GameObject sabotageElectricity;

    public static ShowPuzzle Instance = null;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        print("Loading puzzle manager");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Algo salio mal, hay dos instancias actualmente");
        }

        VotingManager.OnVotingStarted += StopCurrentPuzzle;
    }

    private void StopCurrentPuzzle()
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
        sabotagePresure.SetActive(false);
        sabotageElectricity.SetActive(false);
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();

        Instance = null;

        VotingManager.OnVotingStarted -= StopCurrentPuzzle;
    }

    [Client]
    public void OpenPuzzles(PuzzleId puzzle)
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
            case PuzzleId.SabotageBoilerPressure:
                sabotagePresure.SetActive(true);
                break;
            case PuzzleId.SabotageElectricity:
                sabotageElectricity.SetActive(true);
                break;
        }
    }
}
