using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerStats : MonoBehaviour
{
    public Stats localPlayerStats; 
    
    void Start()
    {
        HamiltonHub.Instance.OnStatsUpdate += Instance_OnStatsUpdate;
    }

    private void Instance_OnStatsUpdate(Assets.Scripts.Multiplayer.ServerRequestsPayload.NewStats newStats)
    {
        if(HamiltonHub.Instance.SelectedCharacter == newStats.PlayerName)
        {
            localPlayerStats.Bravery = newStats.Stats.Bravery;
            localPlayerStats.Intelligence = newStats.Stats.Intelligence;
            localPlayerStats.Physical = newStats.Stats.Physical;
            localPlayerStats.Sanity = newStats.Stats.Sanity;
        }
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnStatsUpdate -= Instance_OnStatsUpdate;
    }
}
