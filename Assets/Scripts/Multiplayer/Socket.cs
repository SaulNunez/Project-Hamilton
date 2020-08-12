using UnityEngine;
using HybridWebSocket;
using System.Text;
using System;

public class Socket : MonoBehaviour
{
    public static Socket Instance;

    WebSocket ws;

    public void EnterLobby(EnterLobbyPayload payload)
    {

    }

    public void GetAvailableCharacters()
    {
        ws.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(
        new
        {
            type = "get_available_characters"
        })));
    }

    public void SelectCharacter(SelectCharacterPayload payload)
    {

    }

    public delegate void LobbyJoinedDelegate(LobbyJoinedData data, string error = null);
    public event LobbyJoinedDelegate LobbyJoinedEvent;
    public delegate void AvailableCharactersUpdateDelegate(AvailableCharactersData data, string error = null);
    public event AvailableCharactersUpdateDelegate AvailableCharactersUpdateEvent;
    public delegate void PlayerSelectedCharacterDelegate(PlayerSelectedCharacterData data, string error = null);
    public event PlayerSelectedCharacterDelegate PlayerSelectedCharacterEvent;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Only a single instance of Socket is needed, killing this.");
            Destroy(this);
        }
        var webHostUrl = new Uri(Application.absoluteURL);

        ws = WebSocketFactory.CreateInstance($"wss://{webHostUrl.Host}");

        // Add OnOpen event listener
        ws.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            Debug.Log("WS state: " + ws.GetState().ToString());

            ws.Send(Encoding.UTF8.GetBytes("Hello from Unity 3D!"));
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) =>
        {
            Debug.Log("WS received message: " + Encoding.UTF8.GetString(msg));

        };

        // Add OnError event listener
        ws.OnError += (string errMsg) =>
        {
            Debug.Log("WS error: " + errMsg);
        };

        // Add OnClose event listener
        ws.OnClose += (WebSocketCloseCode code) =>
        {
            Debug.Log("WS closed with code: " + code.ToString());
        };

        // Connect to the server
        ws.Connect();

    }

    private void OnDestroy()
    {
        ws.Close();
    }
}
