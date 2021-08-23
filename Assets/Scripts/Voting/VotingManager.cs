using Extensions;
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
    [Command(requiresAuthority = false)]
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
        print($"{sender.identity.GetComponent<PlayerName>().Name} casted vote for {playerVoted.GetComponent<PlayerName>().Name}");
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

        var votesForPlayer = new Dictionary<GameObject, int>();

        foreach (var player in voting.Values)
        {
            if (votesForPlayer.ContainsKey(player))
            {
                votesForPlayer[player]++;
            }
            else
            {
                votesForPlayer.Add(player, 1);
            }
        }

        this.SuperPrint($"Voting results:\n{JsonUtility.ToJson(votesForPlayer)}");

        int skipVotes = 0;
        //Hay jugadores que no votaron porque no eligieron nada a tiempo, estos por default votan por pasar votacion
        if (voting.Count < NetworkManager.singleton.numPlayers)
        {
            skipVotes = NetworkManager.singleton.numPlayers - voting.Count();

        }

        if(votesForPlayer.Count == 0)
        {
            Messages.Instance.ShowMessageToAll("Ningun jugador se ha expulsado");
            this.SuperPrint("No player has been voted out.");
            return;
        }

        var votesByOrder = votesForPlayer.OrderByDescending(x => x.Value);
        var mostVotedKV = votesByOrder.First();

        if (skipVotes >= mostVotedKV.Value)
        {
            //Most players skipped or not voted

            Messages.Instance.ShowMessageToAll("Ningun jugador se ha expulsado");
            this.SuperPrint("No player has been voted out.");
            return;
        }

        if (votesByOrder.Count() >= 2 && votesByOrder.ElementAt(0).Value == votesByOrder.ElementAt(1).Value)
        {
            Messages.Instance.ShowMessageToAll($"Empate, ningun jugador es votado para ser expulsado");
            this.SuperPrint($"Tie, no player has been voted out.");
        }

        var mostVotedPlayerGameObject = mostVotedKV.Key;

        if (mostVotedPlayerGameObject != null)
        {
            var dedPlayerName = mostVotedPlayerGameObject.GetComponent<PlayerName>();
            var killing = mostVotedPlayerGameObject.GetComponent<Killing>();
            var assasinMessage = killing.canKillSomeone ? "Un asesino fuera." : "No era asesino.";
            Messages.Instance.ShowMessageToAll($"{dedPlayerName.Name} fue votado para ser expulsado. {assasinMessage}");
            this.SuperPrint($"{dedPlayerName.Name} has been voted out.");

            var dedPlayerDie = mostVotedPlayerGameObject.GetComponent<Die>();
            dedPlayerDie.SetSimpleDeath();
        }
    }
}
