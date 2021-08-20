using Mirror;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Extension to the network room manager, handles handling settings done by user from network player to player as well as providing several new events
/// </summary>
public class HamiltonNetworkRoomManager : NetworkRoomManager
{
    /// <summary>
    /// When we changed were we set player name
    /// Network room player used this name to keep tabs on who is currently using a character
    /// So to fix breakage rapidly,  I added a uniquenessId.
    /// That it's not more than a Guid casted to string
    /// Collision is pretty much impossible
    /// </summary>
    private string uniquenessId;

    public string UniquenessId { get => uniquenessId; }

    /// <summary>
    /// Called when scene changes to between lobby to game. 
    /// Should work also if leaving lobby to main screen.
    /// Or on end game to main screen.
    /// </summary>
    public event Action OnSceneChanged;

    public override void OnRoomStartClient()
    {
        base.OnRoomStartClient();

        uniquenessId = Guid.NewGuid().ToString();
    }

    public override void OnRoomClientSceneChanged(NetworkConnection conn)
    {
        OnSceneChanged?.Invoke();
    }

    /// <summary>
    /// Passing information between room player to game player
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns></returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        var networkPlayer = roomPlayer.GetComponent<HamiltonNetworkPlayer>();

        var killingComponent = gamePlayer.GetComponent<Killing>();
        killingComponent.IsAssasin = networkPlayer.isImpostor;

        var characterSelected = networkPlayer.characterType;
        print($"{networkPlayer.characterType}");

        var playerSkinSetup = gamePlayer.GetComponent<PlayerSkin>();
        playerSkinSetup.characterSelected = characterSelected;

        var playerName = gamePlayer.GetComponent<PlayerName>();
        playerName.Name = networkPlayer.name;

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    /// <summary>
    /// In this custom implementation upon all players ready, they are randomly selected to become impostors
    /// </summary>
    public override void OnRoomServerPlayersReady()
    {
        var networkPlayers = roomSlots.Select(p => p as HamiltonNetworkPlayer);

        // Reset flags for every player, I'm not sure if the network player is reused if on several games together
        foreach (var networkPlayer in networkPlayers)
        {
            networkPlayer.isImpostor = false;
        }

        var hubConfigGO = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        var hubConfig = hubConfigGO.GetComponent<HubConfig>();

        var selectedImpostors = networkPlayers.PickRandom(hubConfig.numberOfImpostors);

        foreach (var networkPlayer in selectedImpostors)
        {
            networkPlayer.isImpostor = true;
        }

        base.OnRoomServerPlayersReady();
    }
}
