using UnityEngine;
using NativeWebSocket;
using System.Text;
using System;
using Assets.Scripts.Multiplayer.ResultPayload;

public class Socket : MonoBehaviour
{
    public static Socket Instance;

    WebSocket websocket;

    public void EnterLobby(EnterLobbyPayload payload) =>
        websocket?.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(new
        {
            type = "enter_lobby",
            payload
        })));

    public void GetAvailableCharacters() =>
        websocket?.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(
        new
        {
            type = "get_available_characters"
        })));

    public void SelectCharacter(SelectCharacterPayload payload) =>
        websocket?.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(new
        {
            type = "select_character",
            payload
        })));

    public delegate void LobbyJoinedDelegate(LobbyJoinedData data, string error = null);
    public event LobbyJoinedDelegate LobbyJoinedEvent;
    public delegate void AvailableCharactersUpdateDelegate(AvailableCharactersData data, string error = null);
    public event AvailableCharactersUpdateDelegate AvailableCharactersUpdateEvent;
    public delegate void PlayerSelectedCharacterDelegate(string error = null);
    public event PlayerSelectedCharacterDelegate PlayerSelectedCharacterEvent;

    private async void Start()
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

        websocket = WebSocketFactory.CreateInstance($"wss://{webHostUrl.Host}/gameapi");

        websocket.OnOpen += () =>
        {
            Debug.Log("WS connected!");
        };

        websocket.OnMessage += (byte[] msg) =>
        {
            var messageContents = Encoding.UTF8.GetString(msg);

            var messageTypeObj = JsonUtility.FromJson<EventData>(messageContents);

            switch (messageTypeObj.type)
            {
                case "lobby_joined":
                    InvokeLobbyJoinedEvent(messageContents);
                    break;
                case "available_characters_update":
                    InvokeAvailableCharactersUpdate(messageContents);
                    break;
                case "player_selection_sucess":
                    InvokePlayerSelectionSuccess(messageContents);
                    break;
            }
        };

        websocket.OnError += (string errMsg) =>
        {
            Debug.Log("WS error: " + errMsg);
        };

        websocket.OnClose += (WebSocketCloseCode code) =>
        {
            Debug.Log("WS closed with code: " + code.ToString());
        };

        await websocket.Connect();

    }

    private void InvokePlayerSelectionSuccess(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<PlayerSelectedCharacterData>>(messageContents);
        PlayerSelectedCharacterEvent?.Invoke(eventData.error);
    }

    private void InvokeAvailableCharactersUpdate(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<AvailableCharactersData>>(messageContents);
        if (eventData.error != null)
        {
            AvailableCharactersUpdateEvent?.Invoke(eventData.payload);
        }
        else
        {
            AvailableCharactersUpdateEvent?.Invoke(null, eventData.error);
        }
    }

    private void InvokeLobbyJoinedEvent(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<LobbyJoinedData>>(messageContents);
        if (eventData.error != null)
        {
            LobbyJoinedEvent?.Invoke(eventData.payload);
        }
        else
        {
            LobbyJoinedEvent?.Invoke(null, eventData.error);
        }
    }

    private async void OnDestroy()
    {
        await websocket.Close();
    }
}
