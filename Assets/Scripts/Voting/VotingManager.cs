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
            this.SuperPrint("Can't vote because no voting has been invoked.");
            return;
        }

        var isDeadComponent = sender.identity.GetComponent<Die>();
        this.SuperPrint($"Player: ${isDeadComponent.GetComponent<PlayerName>().Name} is dead: {isDeadComponent.isDead}");
        if (isDeadComponent != null && isDeadComponent.isDead)
        {
            this.SuperPrint("Dead men tell no tales");
            return;
        }
        if(playerVoted != null)
        {
            this.SuperPrint($"Player: ${playerVoted.GetComponent<PlayerName>().Name} is dead: {playerVoted.GetComponent<Die>().isDead}");
        }
        if (playerVoted != null && playerVoted.GetComponent<Die>().isDead)
        {
            this.SuperPrint("Player is dead");
            return;
        }

        if (voting.ContainsKey(sender))
        {
            this.SuperPrint("Player has already voted");
            return;
        }

        voting.Add(sender, playerVoted);
        print($"{sender.identity.GetComponent<PlayerName>().Name} casted vote for {(playerVoted == null? "skip": playerVoted.GetComponent<PlayerName>().Name)}");
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
            if(player != null)
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
        }

        this.SuperPrint($"Voting results, {voting.Count()} voted for: {JsonUtility.ToJson(voting.Values.Select(x=> x.GetComponent<PlayerName>().Name))}");

        int skipVotes = skipVotes = NetworkManager.singleton.numPlayers - voting.Count();

        var votesByOrder = votesForPlayer.OrderByDescending(x => x.Value);
        // this.SuperPrint($"Votes tally: first {votesByOrder.ElementAt(0).Value}, second {votesByOrder.ElementAt(1).Value}");
        var mostVotedKV = votesByOrder.FirstOrDefault();
        var mostVotedPlayerGameObject = mostVotedKV.Key;

        if (votesByOrder.Count() >= 2 && votesByOrder.ElementAt(0).Value == votesByOrder.ElementAt(1).Value)
        {
            Messages.Instance.ShowMessageToAll($"Empate, ningun jugador es votado para ser expulsado");
            this.SuperPrint($"Tie, no player has been voted out.");
        }
        else if (mostVotedPlayerGameObject != null)
        {
            var dedPlayerName = mostVotedPlayerGameObject.GetComponent<PlayerName>();
            var killing = mostVotedPlayerGameObject.GetComponent<Killing>();
            var assasinMessage = killing.canKillSomeone ? "Un asesino fuera." : "No era asesino.";
            Messages.Instance.ShowMessageToAll($"{dedPlayerName.Name} fue votado para ser expulsado. {assasinMessage}");
            this.SuperPrint($"{dedPlayerName.Name} has been voted out.");

            var dedPlayerDie = mostVotedPlayerGameObject.GetComponent<Die>();
            dedPlayerDie.SetSimpleDeath();
        }
        //Hasta el ultimo para que si solo alguien vota por un jugador, se expulse
        else if (votesForPlayer.Count == 0 || skipVotes >= mostVotedKV.Value)
        {
            Messages.Instance.ShowMessageToAll("Ningun jugador se ha expulsado");
            this.SuperPrint("No player has been voted out.");
        }
    }
}
