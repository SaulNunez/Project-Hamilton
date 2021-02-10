using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VotingManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnVoteChanged))]
    private bool canVote = false;

    [SerializeField]
    private GameObject votingScreen;

    private Dictionary<NetworkConnection, GameObject> voting = new Dictionary<NetworkConnection, GameObject>();
    private double votingStartTime;

    [SyncVar(hook = nameof(TimeRemainingChanged))]
    private int timeRemaining = 100;

    public static event Action<int> CurrentTimeRemaining;

    void OnVoteChanged(bool oldValue, bool newValue)
    {
        votingScreen.SetActive(newValue);
    }

    void TimeRemainingChanged(int oldValue, int newValue)
    {
        CurrentTimeRemaining?.Invoke(newValue);
    }

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
        StartCoroutine(nameof(WaitForVoting));

        RpcInvokeStartVotingEvent();
    }

    public static event Action OnVotingStarted;

    [ClientRpc]
    public void RpcInvokeStartVotingEvent()
    {
        OnVotingStarted?.Invoke();
    }

    [Command(ignoreAuthority = true)]
    public void CmdCastVote(GameObject playerVoted, NetworkConnectionToClient sender = null)
    {
        if (canVote)
        {
            var playerDeathComponent = playerVoted.GetComponent<Die>();
            if (playerDeathComponent != null && playerDeathComponent.isDead)
            {
                return;
            }

            if (voting.ContainsKey(sender))
            {
                return;
            }

            voting.Add(sender, playerVoted);
        }
    }

    [Server]
    private IEnumerator WaitForVoting()
    {
        while(timeRemaining > 0)
        {
            timeRemaining--;
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

        foreach(var player in votedPlayers)
        {
            votesForPlayer.Add(player, voting.Values.Count(p => p == player));
        }

        print($"Voting results: {JsonUtility.ToJson(votesForPlayer)}");

        //Hay jugadores que no votaron porque no eligieron nada a tiempo, estos por default votan por pasar votacion
        if(voting.Count < NetworkManager.singleton.numPlayers)
        {
            if (votesForPlayer.ContainsKey(null))
            {
                votesForPlayer[null] = votesForPlayer[null] + NetworkManager.singleton.numPlayers - voting.Count();
            } else
            {
                votesForPlayer[null] = NetworkManager.singleton.numPlayers - voting.Count();
            }
        }

        var getMostVotedPlayerGameObject = votesForPlayer.OrderByDescending(x => x.Value).First().Key;

        if(getMostVotedPlayerGameObject != null)
        {
            var dedPlayer = getMostVotedPlayerGameObject.GetComponent<Die>();
            dedPlayer.SetSimpleDeath();
        } else
        {

        }
    }
}
