using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPuzzle : NetworkBehaviour
{
    [Header("Puzzles")]
    [SerializeField]
    GameObject boilerIntPrefab;
    [SerializeField]
    GameObject sequencePrefab;
    [SerializeField]
    GameObject variablesStringPrefab;
    [SerializeField]
    GameObject variablesBoolPrefab;
    [SerializeField]
    GameObject doWhileCarStarterPrefab;
    [SerializeField]
    GameObject floatThermostatPrefab;
    [SerializeField]
    GameObject forWashingPrefab;
    [SerializeField]
    GameObject whileFillWaterBucketPrefab;
    [SerializeField]
    GameObject ifPickFlowerPrefab;
    [SerializeField]
    GameObject substringPrefab;
    [SerializeField]
    GameObject ifelseCakePrefab;
    [SerializeField]
    GameObject sabotagePresurePrefab;
    [SerializeField]
    GameObject sabotageElectricityPrefab;

    [SyncVar]
    GameObject boilerInt;
    [SyncVar]
    GameObject sequence;
    [SyncVar]
    GameObject variablesString;
    [SyncVar]
    GameObject variablesBool;
    [SyncVar]
    GameObject doWhileCarStarter;
    [SyncVar]
    GameObject floatThermostat;
    [SyncVar]
    GameObject forWashing;
    [SyncVar]
    GameObject whileFillWaterBucket;
    [SyncVar]
    GameObject ifPickFlower;
    [SyncVar]
    GameObject substring;
    [SyncVar]
    GameObject ifelseCake;
    [SyncVar]
    GameObject sabotagePresure;
    [SyncVar]
    GameObject sabotageElectricity;

    public override void OnStartClient()
    {
        base.OnStartClient();

        VotingManager.OnVotingStarted += StopCurrentPuzzle;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var canvas = GameObject.FindGameObjectWithTag(Tags.UiManager);

        var boilerIntInstance = Instantiate(boilerIntPrefab, canvas.transform);
        NetworkServer.Spawn(boilerIntInstance, gameObject);
        boilerInt = boilerIntInstance;

        var sequenceInstance = Instantiate(sequencePrefab, canvas.transform);
        NetworkServer.Spawn(sequenceInstance, gameObject);
        sequence = sequenceInstance;

        var variablesStringInstance = Instantiate(variablesStringPrefab, canvas.transform);
        NetworkServer.Spawn(variablesStringInstance, gameObject);
        variablesString = variablesStringInstance;

        var variablesBoolInstance = Instantiate(variablesBoolPrefab, canvas.transform);
        NetworkServer.Spawn(variablesBoolInstance, gameObject);
        variablesBool = variablesBoolInstance;

        var doWhileCarStarterInstance = Instantiate(doWhileCarStarterPrefab, canvas.transform);
        NetworkServer.Spawn(doWhileCarStarterInstance, gameObject);
        doWhileCarStarter = doWhileCarStarterInstance;

        var floatThermostatInstance = Instantiate(floatThermostatPrefab, canvas.transform);
        NetworkServer.Spawn(floatThermostatInstance, gameObject);
        floatThermostat = floatThermostatInstance;

        var forWashingInstance = Instantiate(forWashingPrefab, canvas.transform);
        NetworkServer.Spawn(forWashingInstance, gameObject);
        forWashing = forWashingInstance;

        var whileFillWaterBucketInstance = Instantiate(whileFillWaterBucketPrefab, canvas.transform);
        NetworkServer.Spawn(whileFillWaterBucketInstance, gameObject);
        whileFillWaterBucket = whileFillWaterBucketInstance;

        var ifPickFlowerInstance = Instantiate(ifPickFlowerPrefab, canvas.transform);
        NetworkServer.Spawn(ifPickFlowerInstance, gameObject);
        ifPickFlower = ifPickFlowerInstance;

        var substringInstance = Instantiate(substringPrefab, canvas.transform);
        NetworkServer.Spawn(substringInstance, gameObject);
        substring = substringInstance;

        var ifelseCakeInstance = Instantiate(ifelseCakePrefab, canvas.transform);
        NetworkServer.Spawn(ifelseCakeInstance, gameObject);
        ifelseCake = ifelseCakeInstance;

        var sabotagePresureInstance = Instantiate(sabotagePresurePrefab, canvas.transform);
        NetworkServer.Spawn(sabotagePresureInstance, gameObject);
        sabotagePresure = sabotagePresureInstance;

        var sabotageElectricityInstance = Instantiate(sabotageElectricityPrefab, canvas.transform);
        NetworkServer.Spawn(sabotageElectricityInstance, gameObject);
        sabotageElectricity = sabotageElectricityInstance;
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

    public override void OnStopClient()
    {
        base.OnStopClient();

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
