using Assets.Scripts.Multiplayer.ResultPayload;
using Assets.Scripts.Multiplayer.ServerRequestsPayload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScreen : MonoBehaviour
{
    public Text statBraveryText;
    public Text statIntelligenceText;
    public Text statPhysicalText;
    public Text statSanityText;

    public Text character;

    private void Start()
    {
        HamiltonHub.Instance.OnStatsUpdate += StatsUpdate;
        HamiltonHub.Instance.OnTurnHasStarted += TurnHasChangedPlayer;
    }

    private void OnEnable()
    {
        var currentPlayer = HamiltonHub.Instance.playersInLobby.Find(x => x.characterPrototype == HamiltonHub.Instance.SelectedCharacter);

        statBraveryText.text = currentPlayer.stats.Bravery.ToString();
        statIntelligenceText.text = currentPlayer.stats.Intelligence.ToString();
        statPhysicalText.text = currentPlayer.stats.Physical.ToString();
        statSanityText.text = currentPlayer.stats.Sanity.ToString();
    }

    private void TurnHasChangedPlayer(NewTurnInformation newPlayerInfo)
    {
        character.text = newPlayerInfo.DisplayName;
    }

    private void StatsUpdate(NewStats newStats)
    {
        if(newStats.PlayerName == HamiltonHub.Instance.SelectedCharacter)
        {
            statBraveryText.text = newStats.Stats.Bravery.ToString();
            statIntelligenceText.text = newStats.Stats.Intelligence.ToString();
            statPhysicalText.text = newStats.Stats.Physical.ToString();
            statSanityText.text = newStats.Stats.Sanity.ToString();
        }
    }
}
