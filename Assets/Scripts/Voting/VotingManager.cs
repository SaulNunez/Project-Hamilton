using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VotingManager : NetworkBehaviour
{
    [SyncVar]
    public bool canVote = false;

    private Dictionary<NetworkConnection, GameObject> voting = new Dictionary<NetworkConnection, GameObject>();
    private double votingStartTime;

    [SyncVar]
    public int timeRemaining = 100;

    /// <summary>
    /// Event called everytime a second goes down on voting time.
    /// </summary>
    /// <remarks>
    /// <strong>Only called on server</strong>
    /// </remarks>
    public static event Action<int> CurrentTimeRemaining;

    /// <summary>
    /// Event called when time ends for voting.
    /// </summary>
    /// <remarks>
    /// <strong>Only called on server</strong>
    /// </remarks>
    public static event Action OnVotingEnded;

    HubConfig config;

    public override void OnStartServer()
    {
        base.OnStartServer();

        config = GameObject.FindGameObjectWithTag(Tags.HubConfig).GetComponent<HubConfig>();
        timeRemaining = config.secondsForVoting;
    }

    [Server]
    public void StartVoting()
    {
        voting.Clear();
        votingStartTime = NetworkTime.time;
        timeRemaining = config.secondsForVoting;
        canVote = true;

        OnVotingStarted?.Invoke(config.secondsForVoting);

        StartCoroutine(nameof(WaitForVoting));
    }

    /// <summary>
    /// Event called when starting voting.
    /// </summary>
    /// <remarks>
    /// <strong>Only called on server</strong>
    /// </remarks>
    public static event Action<int> OnVotingStarted;

    /// <summary>
    /// Cast vote.
    /// </summary>
    /// <remarks>
    /// Can be called several times bacause only first vote counts
    /// </remarks>
    /// <param name="playerVoted">Gameobject of player voted as killer</param>
    /// <param name="sender"></param>
    [Command(ignoreAuthority = true)]
    public void CmdCastVote(GameObject playerVoted, NetworkConnectionToClient sender = null)
    {
        if (!canVote)
        {
            print("Can't vote because no voting has been invoked.");
            return;
        }

        if (playerVoted != null && playerVoted.GetComponent<Die>().isDead)
        {
            print("Player is dead");
            return;
        }

        if (voting.ContainsKey(sender))
        {
            print("Player has already voted");
            return;
        }

        voting.Add(sender, playerVoted);
        print("Casted vote");
    }

    [Server]
    private IEnumerator WaitForVoting()
    {
        while (timeRemaining > 0)
        {
            timeRemaining--;
            CurrentTimeRemaining?.Invoke(timeRemaining);
            yield return new WaitForSeconds(1f);
        }

        OnEndVoting();
    }

    [Server]
    private void OnEndVoting()
    {
        canVote = false;

        OnVotingEnded?.Invoke();

        var votedPlayers = voting.Values.Distinct();
        var votesForPlayer = new Dictionary<GameObject, int>();

        foreach (var player in votedPlayers)
        {
            votesForPlayer.Add(player, voting.Values.Count(p => p == player));
        }

        print($"Voting results: {votesForPlayer.Count}, {voting.Count}");

        int skipVotes = 0;
        //Hay jugadores que no votaron porque no eligieron nada a tiempo, estos por default votan por pasar votacion
        if (voting.Count < NetworkManager.singleton.numPlayers)
        {
            skipVotes = NetworkManager.singleton.numPlayers - voting.Count();

        }
        var mostVotedKV = votesForPlayer.OrderByDescending(x => x.Value).First();

        if (skipVotes >= mostVotedKV.Value)
        {
            //Mas jugadores votaron por expulsar a nadie
            return;
        }

        var getMostVotedPlayerGameObject = mostVotedKV.Key;

        if (getMostVotedPlayerGameObject != null)
        {
            var dedPlayer = getMostVotedPlayerGameObject.GetComponent<Die>();
            dedPlayer.SetSimpleDeath();
        }
    }
}
