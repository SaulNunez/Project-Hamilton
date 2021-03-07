using Mirror;
using System;
using UnityEngine;

/// <summary>
/// <strong>Se debe agregar al jugador</strong>
/// Spawnea una ventana de votación para el
/// </summary>
public class VotingScreenLink : NetworkBehaviour
{
    readonly Lazy<GameObject> instancedVotingScreen =
        new Lazy<GameObject>(() => GameObject.FindGameObjectWithTag(Tags.UiManager).GetComponent<UiElements>().votingScreen);

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += RpcOpenVotingScreen;
        VotingManager.OnVotingEnded += RpcClosingVotingScreen;
    }

    [ClientRpc]
    private void RpcOpenVotingScreen(int _)
    {
        if (hasAuthority)
        {
            print("Show voting screen");
            instancedVotingScreen.Value.SetActive(true);
        }
    }

    [ClientRpc]
    private void RpcClosingVotingScreen()
    {
        if (hasAuthority)
        {
            print("Hide voting screen");
            instancedVotingScreen.Value.SetActive(false);
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= RpcOpenVotingScreen;
        VotingManager.OnVotingEnded -= RpcClosingVotingScreen;
    }
}
