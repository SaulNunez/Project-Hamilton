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

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += StopCurrentPuzzle;

        var canvas = GameObject.FindGameObjectWithTag(Tags.UiManager);

        var boilerIntInstance = Instantiate(boilerIntPrefab, canvas.transform);
        NetworkServer.Spawn(boilerIntInstance, netIdentity.connectionToServer);
        boilerInt = boilerIntInstance;

        var sequenceInstance = Instantiate(sequencePrefab, canvas.transform);
        NetworkServer.Spawn(sequenceInstance, netIdentity.connectionToServer);
        sequence = sequenceInstance;

        var variablesStringInstance = Instantiate(variablesStringPrefab, canvas.transform);
        NetworkServer.Spawn(variablesStringInstance, netIdentity.connectionToServer);
        variablesString = variablesStringInstance;

        var variablesBoolInstance = Instantiate(variablesBoolPrefab, canvas.transform);
        NetworkServer.Spawn(variablesBoolInstance, netIdentity.connectionToServer);
        variablesBool = variablesBoolInstance;

        var doWhileCarStarterInstance = Instantiate(doWhileCarStarterPrefab, canvas.transform);
        NetworkServer.Spawn(doWhileCarStarterInstance, netIdentity.connectionToServer);
        doWhileCarStarter = doWhileCarStarterInstance;

        var floatThermostatInstance = Instantiate(floatThermostatPrefab, canvas.transform);
        NetworkServer.Spawn(floatThermostatInstance, netIdentity.connectionToServer);
        floatThermostat = floatThermostatInstance;

        var forWashingInstance = Instantiate(forWashingPrefab, canvas.transform);
        NetworkServer.Spawn(forWashingInstance, netIdentity.connectionToServer);
        forWashing = forWashingInstance;

        var whileFillWaterBucketInstance = Instantiate(whileFillWaterBucketPrefab, canvas.transform);
        NetworkServer.Spawn(whileFillWaterBucketInstance, netIdentity.connectionToServer);
        whileFillWaterBucket = whileFillWaterBucketInstance;

        var ifPickFlowerInstance = Instantiate(ifPickFlowerPrefab, canvas.transform);
        NetworkServer.Spawn(ifPickFlowerInstance, netIdentity.connectionToServer);
        ifPickFlower = ifPickFlowerInstance;

        var substringInstance = Instantiate(substringPrefab, canvas.transform);
        NetworkServer.Spawn(substringInstance, netIdentity.connectionToServer);
        substring = substringInstance;

        var ifelseCakeInstance = Instantiate(ifelseCakePrefab, canvas.transform);
        NetworkServer.Spawn(ifelseCakeInstance, netIdentity.connectionToServer);
        ifelseCake = ifelseCakeInstance;

        var sabotagePresureInstance = Instantiate(sabotagePresurePrefab, canvas.transform);
        NetworkServer.Spawn(sabotagePresureInstance, netIdentity.connectionToServer);
        sabotagePresure = sabotagePresureInstance;

        var sabotageElectricityInstance = Instantiate(sabotageElectricityPrefab, canvas.transform);
        NetworkServer.Spawn(sabotageElectricityInstance, netIdentity.connectionToServer);
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

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= StopCurrentPuzzle;
    }

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
                case PuzzleId.SabotageBoilerPressure:
                    sabotagePresure.SetActive(true);
                    break;
                case PuzzleId.SabotageElectricity:
                    sabotageElectricity.SetActive(true);
                    break;
            }
        }
    }
}
