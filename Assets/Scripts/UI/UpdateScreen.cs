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

    private void Start()
    {
        HamiltonHub.Instance.OnStatsUpdate += StatsUpdate;
    }

    private void StatsUpdate(Assets.Scripts.Multiplayer.ServerRequestsPayload.NewStats newStats)
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
