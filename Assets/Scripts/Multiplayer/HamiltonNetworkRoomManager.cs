using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Extension to the network room manager, handles handling settings done by user from network player to player as well as providing several new events
/// </summary>
public class HamiltonNetworkRoomManager : NetworkRoomManager
{
    public event Action OnSceneChanged;

    public string PlayerName {get; set;}
    
    public override void OnRoomClientSceneChanged(NetworkConnection conn)
    {
        OnSceneChanged?.Invoke();
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        var networkPlayer = roomPlayer.GetComponent<HamiltonNetworkPlayer>();

        var killingComponent = gamePlayer.GetComponent<Killing>();
        killingComponent.IsAssasin = networkPlayer.isImpostor;

        var playerSelectionComponent = roomPlayer.GetComponent<PlayerSelectionInLobby>();
        var characterSelected = playerSelectionComponent.currentCharacterType;

        var playerSkinSetup = gamePlayer.GetComponent<PlayerSkin>();
        playerSkinSetup.characterSelected = characterSelected;

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    /// <summary>
    /// In this custom implementation upon all players ready, they are randomly selected to become impostors
    /// </summary>
    public override void OnRoomServerPlayersReady()
    {
        var networkPlayers = roomSlots.Select(p => p as HamiltonNetworkPlayer);

        // Reset flags for every player, I'm not sure if the network player is reused if on several games together
        foreach(var networkPlayer in networkPlayers)
        {
            networkPlayer.isImpostor = false;
        }

        var hubConfigGO = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        var hubConfig = hubConfigGO.GetComponent<HubConfig>();

        var selectedImpostors = networkPlayers.PickRandom(hubConfig.numberOfImpostors);
 
        foreach(var networkPlayer in selectedImpostors)
        {
            networkPlayer.isImpostor = true;
        }

        base.OnRoomServerPlayersReady();
    }
}
