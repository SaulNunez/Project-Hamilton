using Extensions;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotingScreenManager : NetworkBehaviour
{
    [SerializeField]
    GameObject votingScreen;

    /// <summary>
    /// Button prefab, to be spawned
    /// </summary>
    [SerializeField]
    private GameObject playerButtonPrefab;

    /// <summary>
    /// Gameobject to attach the buttons
    /// </summary>
    [SerializeField]
    private GameObject playerButtonParent;

    /// <summary>
    /// Button to listen for skip
    /// </summary>
    [SerializeField]
    private Button skipButton;

    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingStarted += RpcOpenVotingScreen;
        VotingManager.OnVotingEnded += RpcClosingVotingScreen;
    }

    [ClientRpc]
    private void RpcOpenVotingScreen(int _)
    {
        this.SuperPrint("Show voting screen");
        votingScreen.SetActive(true);
        OnScreenEnabled();

        var weDed = NetworkClient.localPlayer.GetComponent<Die>().isDead;
        skipButton.interactable = !weDed;
    }

    [ClientRpc]
    private void RpcClosingVotingScreen()
    {
        this.SuperPrint("Hide voting screen");
        votingScreen.SetActive(false);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingStarted -= RpcOpenVotingScreen;
        VotingManager.OnVotingEnded -= RpcClosingVotingScreen;
    }

    [Client]
    void OnScreenEnabled()
    {
        var players = GameObject.FindGameObjectsWithTag(Tags.Player);

        //Quitar jugadores existentes
        foreach (Transform child in playerButtonParent.transform)
        {
            if (child != playerButtonParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        var weDed = NetworkClient.localPlayer.GetComponent<Die>().isDead;

        foreach (var player in players)
        {
            var button = Instantiate(playerButtonPrefab, playerButtonParent.transform);

            //Obtener sprites
            var playerSprite = player.GetComponent<SpriteRenderer>().sprite;

            //Get name
            var playerName = player.GetComponentInChildren<PlayerName>().Name;

            //Get dead status
            var isDead = player.GetComponent<Die>().isDead;

            var playerButton = button.GetComponent<PlayerVotingButton>();


            playerButton.PlayerSprite = playerSprite;
            playerButton.Name = playerName;
            playerButton.onSelect.AddListener(() => VoteForPlayer(player));
            button.GetComponent<Button>().interactable = !isDead && !weDed;
        }

    }

    /// <summary>
    /// Utility vote casting method for the buttons.
    /// </summary>
    /// <param name="player"></param>
    void VoteForPlayer(GameObject player)
    {
        foreach (var button in playerButtonParent.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        skipButton.interactable = false;

        var votingManager = GetComponent<VotingManager>();

        votingManager.CmdCastVote(player);
    }

    public void SkipVoting()
    {
        foreach (var button in playerButtonParent.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        skipButton.interactable = false;

        var votingManager = GetComponent<VotingManager>();

        votingManager.CmdCastVote(null);
    }
}
