using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <strong>Se debe agregar al jugador</strong>
/// Spawnea una ventana de votación para el
/// </summary>
public class VotingScreenLink : NetworkBehaviour
{
    [SerializeField]
    GameObject votingScreenPrefab;

    [SyncVar]
    GameObject instancedVotingScreen;

    public override void OnStartServer()
    {
        base.OnStartServer();

        var canvas = GameObject.FindGameObjectWithTag(Tags.UiManager);
        var instanced = Instantiate(votingScreenPrefab, canvas.transform);

        //Spawnear con ownership del jugador
        NetworkServer.Spawn(instanced, gameObject);

        instancedVotingScreen = instanced;

        VotingManager.OnVotingStarted += OpenVotingScreen;
        VotingManager.OnVotingEnded += ClosingVotingScreen;
    }

    [Server]
    private void OpenVotingScreen()
    {
        print("Show puzzle");
        instancedVotingScreen.SetActive(true);
    }

    [Server]
    private void ClosingVotingScreen()
    {
        print("Hide puzzle");
        instancedVotingScreen.SetActive(false);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= OpenVotingScreen;
        VotingManager.OnVotingEnded -= ClosingVotingScreen;
    }
}
