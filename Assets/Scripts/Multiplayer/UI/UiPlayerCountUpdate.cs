using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

public class UiPlayerCountUpdate : MonoBehaviour
{
    NetworkManager lobbyNetworkManager;
    TextMeshProUGUI textbox;

    private void UpdateScreen()
    {
        textbox.text = $"{lobbyNetworkManager.numPlayers}/{lobbyNetworkManager.maxConnections}";
    }

    void Start()
    {
        textbox = GetComponent<TextMeshProUGUI>();

        LobbyNetworkManager.OnClientConnected += UpdateScreen;

        var nmGameObject = GameObject.FindGameObjectWithTag("NetworkManager");
        lobbyNetworkManager = nmGameObject.GetComponent<LobbyNetworkManager>();

        UpdateScreen();
    }

    void OnDestroy()
    {
        LobbyNetworkManager.OnClientConnected -= UpdateScreen;
    }
}
