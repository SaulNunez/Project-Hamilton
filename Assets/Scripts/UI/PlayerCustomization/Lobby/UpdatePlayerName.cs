using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerName : NetworkBehaviour
{
    /// <summary>
    /// Sends a command to server to change name. This name will be visible on top of the player.
    /// </summary>
    /// <remarks>
    /// Must be on a object with authority. 
    /// Must be on a network player prefab.
    /// </remarks>
    /// <param name="newName">New player name</param>
    [Command]
    public void CmdUpdateName(string newName)
    {
        var networkPlayer = GetComponent<HamiltonNetworkPlayer>();
        networkPlayer.playerName = newName;
    }
}
