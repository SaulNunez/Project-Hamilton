using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyConnect : MonoBehaviour
{
    public LobbyNetworkManager lobbyNetworkManager;

    public void CreateLobby()
    {
        lobbyNetworkManager.StartHost();

    }
}
