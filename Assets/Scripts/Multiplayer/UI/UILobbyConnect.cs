using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyConnect : MonoBehaviour
{
    public NetworkManager lobbyNetworkManager;

    public void CreateLobby()
    {
        lobbyNetworkManager.StartServer();

    }
}
